using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Data;
using System.Data.SqlClient;
using wrcaysalesinventory.Data.Classes;
using BCrypt.Net;

namespace wrcaysalesinventory.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private string _userName;
        private string _password;
        private bool  _rememberMe;

        public string UserName { get => _userName; set => Set(ref _userName, value); }
        public string Password { get => _password; set => Set(ref _password, value); }
        public bool   RememberMe { get => _rememberMe; set => Set(ref _rememberMe, value); }

        public RelayCommand<object> LoginCmd => new(LoginCommand);
        private void LoginCommand(object obj)
        {
            SqlConnection sqlConnection = null;
            SqlCommand sqlCommand = null;
            try
            {
                sqlConnection = SqlBaseConnection.GetInstance();
                sqlCommand = new("SELECT password FROM tblusers WHERE username = @username", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@username", UserName);
                SqlDataAdapter adapter = new(sqlCommand);
                DataTable dataTable = new();
                adapter.Fill(dataTable);

                if(dataTable.Rows.Count > 0)
                {
                    if(BCrypt.Net.BCrypt.Verify(Password, dataTable.Rows[0]["password"].ToString()))
                    {

                    } else
                    {
                        MessageBox.Info("Invalid username or password!");
                    }
                } else
                {
                    MessageBox.Info("Username does not exists!");
                }

            }
            catch
            {

            } finally
            {
                sqlConnection?.Close();
            }
        }
    }
}
