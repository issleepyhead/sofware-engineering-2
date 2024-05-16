using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;
using wrcaysalesinventory.Forms;

namespace wrcaysalesinventory.ViewModels
{
    public class ProfileDialogViewModel : ViewModelBase
    {
        private MainWindow mw;
        public ProfileDialogViewModel()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }
        private UserModel _model = new();
        public UserModel Model { get => _model; set => Set(ref _model, value); }

        private string _firstNameError;
        public string FirstNameError { get => _firstNameError; set => Set(ref _firstNameError, value); }

        private string _lastNameError;
        public string LastNameError { get => _lastNameError; set => Set(ref _lastNameError, value); }

        private string _phoneError;
        public string PhoneError { get => _phoneError; set => Set(ref _phoneError, value); }

        private string _addressError;
        public string AddressError { get => _addressError; set => Set(ref _addressError, value); }

        private string _userNameError;
        public string UserNameError { get => _userNameError; set => Set(ref _userNameError, value); }

        private string _roleError;
        public string RoleError { get => _roleError; set => Set(ref _roleError, value); }

        private string _passwordError;
        public string PasswordError { get => _passwordError; set => Set(ref _passwordError, value); }

        public RelayCommand<Button> LogOutCommand => new(LogOutCmd);
        private void LogOutCmd(Button obj)
        {
            GlobalData.Config.UserID = -1;
            GlobalData.Config.RoleID = -1;
            GlobalData.Config.StatusID = -1;
            GlobalData.Config.FullName = string.Empty;
            GlobalData.Save();
            WinHelper.CloseDialog(obj);
            Thread.Sleep(500);
            LogInWindow logInWindow = new();
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mw?.Close();
           
            logInWindow.Show();
        }

        public RelayCommand<object> LoadedCmd => new(LoadedCommand);
        private void LoadedCommand(object obj) { 
            try
            {
                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                SqlCommand sqlCommand = new("SELECT first_name, last_name, address, contact, username FROM tblusers WHERE id = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", GlobalData.Config.UserID);
                SqlDataAdapter adapter = new(sqlCommand);
                DataTable dt = new();
                adapter.Fill(dt);

                UserModel usr = new();
                foreach(DataRow row in dt.Rows)
                {
                    usr.ID = GlobalData.Config.UserID.ToString();
                    usr.FirstName = row["first_name"].ToString();
                    usr.LastName = row["last_name"].ToString();
                    usr.Address = row["address"].ToString();
                    usr.Contact = row["contact"].ToString();
                    usr.Username = row["username"].ToString();
                }
                Model = usr;
            } catch
            {
                Debug.WriteLine("Error at profile dialog.");
            }
        }

        public RelayCommand<object> SaveCommand => new(SaveCmd);
        private void SaveCmd(object obj)
        {
            UserValidator validator = new();
            FluentValidation.Results.ValidationResult result = validator.Validate(Model);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.PropertyName)
                    {
                        case nameof(Model.FirstName):
                            FirstNameError = failure.ErrorMessage;
                            break;
                        case nameof(Model.LastName):
                            LastNameError = failure.ErrorMessage;
                            break;
                        case nameof(Model.Address):
                            AddressError = failure.ErrorMessage;
                            break;
                        case nameof(Model.Contact):
                            PhoneError = failure.ErrorMessage;
                            break;
                        case nameof(Model.Username):
                            UserNameError = failure.ErrorMessage;
                            break;
                        case nameof(Model.Password):
                            PasswordError = failure.ErrorMessage;
                            break;
                    }
                }

                List<string> failureproplist = new();
                foreach (var failure in result.Errors)
                    failureproplist.Add(failure.PropertyName);

                List<string> proplist = new List<string>
                {
                    nameof(UserModel.FirstName),
                    nameof(UserModel.LastName),
                    nameof(UserModel.Address),
                    nameof(UserModel.Contact),
                    nameof(UserModel.Username),
                    nameof(UserModel.Password)
                };
                foreach (var prop in proplist)
                    if (!failureproplist.Contains(prop))
                    {
                        switch (prop)
                        {
                            case nameof(Model.FirstName):
                                FirstNameError = null;
                                break;
                            case nameof(Model.LastName):
                                LastNameError = null;
                                break;
                            case nameof(Model.Address):
                                AddressError = null;
                                break;
                            case nameof(Model.Contact):
                                PhoneError = null;
                                break;
                            case nameof(Model.Username):
                                UserNameError = null;
                                break;
                            case nameof(Model.Password):
                                PasswordError = null;
                                break;
                        }
                    }
            }
            else
            {
                UserNameError = null;
                PasswordError = null;
                LastNameError = null;
                FirstNameError = null;
                AddressError = null;
                PhoneError = null;

                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                SqlCommand sqlCommand;
                try
                {

                    if (string.IsNullOrEmpty(Model.ID))
                    {
                        sqlCommand = new("SELECT COUNT(*) FROM tblusers WHERE username = @usrname", sqlConnection);
                    }
                    else
                    {
                        sqlCommand = new("SELECT COUNT(*) FROM tblusers WHERE username = @usrname AND id != @id", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", Model.ID);
                    }

                    sqlCommand.Parameters.AddWithValue("@usrname", Model.Username);
                    if ((int)sqlCommand.ExecuteScalar() > 0)
                    {
                        Growl.Info("Username exists!");
                        return;
                    }

                        
                    sqlCommand = new(@"UPDATE tblusers SET first_name = @fname, last_name = @lname, address = @address, contact = @contact,
                                               username = @username, password = @password WHERE id = @id", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@id", Model.ID);
                    sqlCommand.Parameters.AddWithValue("@lname", Model.LastName);
                    sqlCommand.Parameters.AddWithValue("@fname", Model.FirstName);
                    sqlCommand.Parameters.AddWithValue("@address", Model.Address);
                    sqlCommand.Parameters.AddWithValue("@contact", Model.Contact);
                    sqlCommand.Parameters.AddWithValue("@username", Model.Username);
                    sqlCommand.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(Model.Password));
                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        Growl.Success("Account has been updated successfully!");
                        mw?.UpdateAll();
                        WinHelper.AuditActivity("UPDATED", "ACCOUNT");
                    }
                    else
                    {
                        Growl.Info("An error occured while performing the action.");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

    }
}
