using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Data;
using System.Data.SqlClient;
using wrcaysalesinventory.Data.Classes;
using BCrypt.Net;
using System.Threading.Tasks;

namespace wrcaysalesinventory.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private string _userName;
        private string _userNameError;
        private string _passwordError;
        private string _password;
        private bool  _rememberMe;
        private string _loginContent = "Login";
        private bool _isLoginEnable = true;

        public string LoginContent { get => _loginContent; set => Set(ref _loginContent, value); }
        public string UserName { get => _userName; set => Set(ref _userName, value); }
        public string UserNameError { get => _userNameError; set => Set(ref _userNameError, value); }
        public string PasswordError { get => _passwordError; set => Set(ref _passwordError, value); }
        public string Password { get => _password; set => Set(ref _password, value); }
        public bool IsLoginEnable { get => _isLoginEnable; set => Set(ref _isLoginEnable, value); }

        public RelayCommand<Window> LoginCmd => new(LoginCommand);
        private async void LoginCommand(Window obj)
        {
            UserNameError = string.Empty;
            PasswordError = string.Empty;
            if((await ProcessLogin()))
            {
                MainWindow mainWindow = new();
                mainWindow.Show();
                UserName = string.Empty;
                Password = string.Empty;
                obj.Close();
                
            }
            IsLoginEnable = true;
            LoginContent = "Login";
        }


        public RelayCommand<Window> LogOutCmd => new(LogOutCommand);
        private async void LogOutCommand(Window obj)
        {
            obj.Close();
        }

        private async Task<bool> ProcessLogin()
        {
            bool res = true;
            await Task.Run(() =>
            {
                LoginContent = "Logging in...";
                IsLoginEnable = false;
                SqlConnection sqlConnection = null;
                SqlCommand sqlCommand = null;
                try
                {
                    sqlConnection = SqlBaseConnection.GetInstance();
                    sqlCommand = new("SELECT u.id, concat(first_name, ' ', last_name) fullname, password, role_id, s.id status_id, s.status_name FROM tblusers u JOIN tblstatus s ON u.status_id = s.id WHERE username = @username", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@username", UserName);
                    SqlDataAdapter adapter = new(sqlCommand);
                    DataTable dataTable = new();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        if (BCrypt.Net.BCrypt.Verify(Password, dataTable.Rows[0]["password"].ToString()))
                        {
                            if (dataTable.Rows[0]["status_name"].ToString().ToLower() == "active")
                            {
                                GlobalData.Config.RoleID = (int)dataTable.Rows[0]["role_id"];
                                GlobalData.Config.UserID = (int)dataTable.Rows[0]["id"];
                                GlobalData.Config.StatusID = (int)dataTable.Rows[0]["status_id"];
                                GlobalData.Config.FullName = dataTable.Rows[0]["fullname"].ToString();
                                GlobalData.Save();
                                // TO-DO Fix some things here pa para sa accs
                                res = true;
                            } else
                            {
                                res = false;
                                UserName = "This account is inactive.";
                            }
                        }
                        else
                        {
                            res = false;
                            UserNameError = "Invalid username or password!";
                        }
                    }
                    else
                    {
                        res = false;
                        UserNameError = "Username does not exists!";
                    }

                }
                catch
                {
                    res = false;
                    PasswordError = "An unknown error occured, please try again.";
                }
                finally
                {
                    sqlConnection?.Close();
                }
            });
            return res;
        }

        public RelayCommand<Window> CloseCmd => new(CloseCommand);
        private void CloseCommand(Window window)
        {
            window.Close();
        }
    }
}
