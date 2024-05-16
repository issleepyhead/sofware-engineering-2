using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Forms;
using wrcaysalesinventory.Properties;

namespace wrcaysalesinventory.ViewModels
{
    public class ServerWindowViewModel : ViewModelBase
    {
        private string _serverName;
        private string _databaseName;
        private string _userName;
        private string _password;

        private string _serverNameError;
        private string _databaseError;
        private string _userNameError;
        private string _passwordError;
        private bool _isEnableConnect = true;
        private DataTable _dataSource;
        private string _testContextText = "Test Connection";

        public string ServerName { get => _serverName; set => Set(ref _serverName, value); }
        public string Database { get => _databaseName; set => Set(ref _databaseName, value); }
        public string UserName { get => _userName; set => Set(ref _userName, value); }
        public string Password { get => _password; set => Set(ref _password, value); }

        public string ServerError { get => _serverNameError; set => Set(ref _serverNameError, value); }
        public string DatabaseError { get => _databaseError; set => Set(ref _databaseError, value); }
        public string UserNameError { get => _userNameError; set => Set(ref _userNameError, value); }
        public string PasswordError { get => _passwordError; set => Set(ref _passwordError, value); }
        public bool IsEnable { get => _isEnableConnect; set => Set(ref _isEnableConnect, value); }
        public DataTable DatabaseSource { get => _dataSource; set => Set(ref _dataSource, value); }
        public string TestContextText { get => _testContextText; set => Set(ref _testContextText, value); }

        public RelayCommand<Window> CloseCmd => new(CloseCommand);
        private void CloseCommand(Window window)
        {
            window.Close();
        }

        public RelayCommand<Window> ConnectCmd => new(ConnectCommand);
        private void ConnectCommand(Window obj)
        {
            ServerError = string.Empty;
            DatabaseError = string.Empty;
            UserNameError = string.Empty;
            PasswordError = string.Empty;
            if (string.IsNullOrEmpty(UserName) ||
                string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(ServerName) ||
                string.IsNullOrEmpty(Database))
            {
                if (string.IsNullOrEmpty(Password))
                    PasswordError = "Please provide a password!";
                if (string.IsNullOrEmpty(UserName))
                    UserNameError = "Please provide a username!";
                if (string.IsNullOrEmpty(ServerName))
                    ServerError = "Please provide a server name.";
                if (string.IsNullOrEmpty(Database))
                    DatabaseError = "Please select a database.";
                return;
            }
            SqlConnection sqlConnection = null;
            try
            {
                if(DatabaseConfig.CheckDatabase($"Server={ServerName};Initial Catalog={Database};Persist Security Info=True;User ID={UserName};Password={Password}"))
                {
                    
                    Settings.Default.connStr = $"Server={ServerName};Initial Catalog={Database};Persist Security Info=True;User ID={UserName};Password={Password}";
                    Settings.Default.Save();

                    sqlConnection = new(Settings.Default.connStr);
                    sqlConnection.Open();

                    // dumbest way to initialize a user.
                    SqlCommand sqlCmd = new("SELECT COUNT(*) FROM tblusers", sqlConnection);
                    if ((int)sqlCmd.ExecuteScalar() == 0)
                    {
                        string password = BCrypt.Net.BCrypt.HashPassword("SA");
                        sqlCmd = new("INSERT INTO tblusers (status_id, role_id, first_name, last_name, address, contact, username, password) VALUES (1, 1, 'SA', 'SA', 'SA', '0912 345 678', 'SA', @pwd)", sqlConnection);
                        sqlCmd.Parameters.AddWithValue("@pwd", password);
                        if (sqlCmd.ExecuteNonQuery() <= 0)
                        {
                            MessageBox.Warning("Database Error.");
                        }
                    }
                    LogInWindow logInWindow = new();
                    logInWindow.Show();
                    obj.Close();
                } else
                {
                    DatabaseError = "The selected database is incorrect.";
                }  
            }
            catch
            {
                UserNameError = "Incorrect username or password!";
            } finally
            {
                sqlConnection?.Close();
            }
        }

        public RelayCommand<object> TestCmd => new(TestCommand);
        private async void TestCommand(object obj)
        {
            ServerError = string.Empty;
            DatabaseError = string.Empty;
            UserNameError = string.Empty;
            PasswordError = string.Empty;

            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ServerName))
            {
                if(string.IsNullOrEmpty(Password))
                    PasswordError = "Please provide a password!";
                if(string.IsNullOrEmpty(UserName))
                    UserNameError = "Please provide a username!";
                if(string.IsNullOrEmpty(ServerName))
                    ServerError = "Please provide a server name.";
                return;
            }

            if((await ConnectServer()))
            {
                ServerError = string.Empty;
                SqlConnection sqlConnection = new($"Server={ServerName};Persist Security Info=True;User ID={UserName};Password={Password};");
                try
                {
                    sqlConnection.Open();
                    SqlCommand sqlCommand = new("SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb');", sqlConnection);
                    DataTable dataTable = new();
                    SqlDataAdapter sqlDataAdapter = new(sqlCommand);
                    sqlDataAdapter.Fill(dataTable);
                    DatabaseSource = dataTable;
                    
                } catch
                {
                    ServerError = "Unable to connect to the server.";
                } finally
                {
                    sqlConnection?.Close();
                }
            }
            else
            {
                ServerError = "Unable to connect to the server";
            }
            TestContextText = "Test Connection";
            IsEnable = true;
        }

        public async Task<bool> ConnectServer()
        {
            IsEnable = false;
            TestContextText = "Connecting...";
            bool result = false;
            await Task.Run(() => {
                SqlConnection sqlConnection = new($"Server={ServerName};Persist Security Info=True;User ID={UserName};Password={Password};");
                try
                {
                    sqlConnection.Open();
                        result = true;
                }
                catch
                {
                    result = false;
                }
            });
            return result;
        }
    }
}
