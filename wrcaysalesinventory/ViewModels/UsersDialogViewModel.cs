using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System;
using System.Windows.Markup;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;
using System.Windows.Documents;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using wrcaysalesinventory.Services;
using System.Collections.ObjectModel;
using MessageBox = HandyControl.Controls.MessageBox;
using wrcaysalesinventory.Properties.Langs;

namespace wrcaysalesinventory.ViewModels
{
    public class UsersDialogViewModel : ViewModelBase
    {
        private DataService _dataService;
        private MainWindow mw;
        private Visibility _dVisibility = Visibility.Collapsed;
        private string _dLabel = Lang.LabelAdd;
        public UsersDialogViewModel(DataService dataService)
        {
            _dataService = dataService;
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        public ObservableCollection<StatusModel> StatusList { get => _dataService.GetStatusList(); }
        public ObservableCollection<RoleModel> RoleList { get => _dataService.GetRoleList(); }

        private UserModel _userModel = new();
        public UserModel Model { get => _userModel;
            set
            { 
                Set(ref _userModel, value);
                DeleteVisibility = string.IsNullOrEmpty(value.ID) ? Visibility.Collapsed : Visibility.Visible;
                ButtonContent = string.IsNullOrEmpty(value.ID) ?
                    Lang.LabelAdd : (value.StatusName.ToLower() == "inactive" ? Lang.LabelRestore : Lang.LabelUpdate);
            }
        }

        private Button _btn;
        public Button BTN { get => _btn; set => Set(ref _btn, value); }

        private string _firstNameError;
        public string FirstNameError { get => _firstNameError; set => Set(ref _firstNameError, value); }

        private string _lastNameError;
        public string LastNameError { get => _lastNameError; set =>Set(ref _lastNameError, value); }

        private string _phoneError;
        public string PhoneError { get => _phoneError; set => Set(ref _phoneError, value); }

        private string _addressError;
        public string AddressError { get => _addressError; set => Set(ref _addressError, value);  }

        private string _userNameError;
        public string UserNameError { get => _userNameError; set => Set(ref _userNameError, value); }

        private string _roleError;
        public string RoleError { get => _roleError; set => Set(ref _roleError, value); }

        private string _passwordError;
        public string PasswordError { get => _passwordError; set => Set(ref _passwordError, value); }


        public Visibility DeleteVisibility { get => _dVisibility; set => Set(ref _dVisibility, value); }
        public string ButtonContent { get => _dLabel; set => Set(ref _dLabel, value); }

        public RelayCommand<UsersDialogViewModel> DeleteCmd => new(DeleteCommand);
        private void DeleteCommand(UsersDialogViewModel vm)
        {
            try
            {
                if(GlobalData.Config.RoleID < int.Parse(vm.Model.RoleID))
                {
                    SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                    if(!string.IsNullOrEmpty(vm.Model.StatusName) && vm.Model.StatusName.ToLower() == "active".ToLower())
                    {
                        SqlCommand sqlCmd = new("UPDATE tblusers SET status_id = (SELECT TOP 1 id FROM tblstatus WHERE status_name = 'Inactive') WHERE id = @id;", sqlConnection);
                        sqlCmd.Parameters.AddWithValue("@id", vm.Model.ID);
                        if (sqlCmd.ExecuteNonQuery() > 0)
                        {
                            Growl.Success("Account has been archived successfully!");
                            WinHelper.CloseDialog(_btn);
                            WinHelper.AuditActivity("ARCHIVED", "USER");
                        }
                        else
                        {
                            Growl.Info("Failed archiving the user.");
                        }
                    } else
                    {
                        SqlCommand sqlCmd = new("DELETE FROM tblusers WHERE id = @id;", sqlConnection);
                        sqlCmd.Parameters.AddWithValue("@id", vm.Model.ID);
                        if (sqlCmd.ExecuteNonQuery() > 0)
                        {
                            Growl.Success("Account has been deleted successfully!");
                            WinHelper.CloseDialog(_btn);
                            WinHelper.AuditActivity("DELETED", "USER");
                        }
                        else
                        {
                            Growl.Info("Failed deleting the user.");
                        }
                    }

                } else
                {
                    Growl.Info("You are not allowed to perform this action.");
                }
                mw?.UpdateAll();
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("DELETE"))
                    MessageBox.Warning(@"This action cannot be completed because the record is referenced by other data in the system. Please remove associated references or contact your system administrator for assistance.", "Unable to delete record.");
                else
                    Growl.Warning("An error occured while performing the action.");
            }
        }

        public RelayCommand<UsersDialogViewModel> ValidateVM => new(ValidateModel);

        private void ValidateModel(UsersDialogViewModel vm)
        {
            UserValidator validator = new();
            FluentValidation.Results.ValidationResult result = validator.Validate(vm.Model);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.PropertyName)
                    {
                        case nameof(Model.FirstName):
                            FirstNameError = failure.ErrorMessage;
                            break;
                        case nameof(vm.Model.LastName):
                            LastNameError = failure.ErrorMessage;
                            break;
                        case nameof(vm.Model.Address):
                            AddressError = failure.ErrorMessage;
                            break;
                        case nameof(vm.Model.Contact):
                            PhoneError = failure.ErrorMessage;
                            break;
                        case nameof(vm.Model.Username):
                            UserNameError = failure.ErrorMessage;
                            break;
                        case nameof(vm.Model.Password):
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
                            case nameof(vm.Model.LastName):
                                LastNameError = null;
                                break;
                            case nameof(vm.Model.Address):
                                AddressError = null;
                                break;
                            case nameof(vm.Model.Contact):
                                PhoneError = null;
                                break;
                            case nameof(vm.Model.Username):
                                UserNameError = null;
                                break;
                            case nameof(vm.Model.Password):
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
                    //sqlCommand = new("SELECT role_id FROM tblusers WHERE id = @id", sqlConnection);
                    //sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                    //if((int)sqlCommand.ExecuteScalar() > int.Parse(vm.Model.RoleID))
                    //{
                    //    Growl.Info("");
                    //    return;
                    //}

                    if(!string.IsNullOrEmpty(vm.Model.ID) && GlobalData.Config.RoleID == int.Parse(vm.Model.RoleID))
                    {
                        //if (GlobalData.Config.RoleID == int.Parse(vm.Model.RoleID))
                        //{
                        Growl.Info("You can't update an account with this role.");
                        return;
                        //}
                    } else if(string.IsNullOrEmpty(vm.Model.ID) && GlobalData.Config.RoleID == int.Parse(vm.Model.RoleID))
                    {
                        Growl.Info("You can't create an account with this role.");
                        return;
                    }

                    if (string.IsNullOrEmpty(vm.Model.ID))
                    {
                        sqlCommand = new("SELECT COUNT(*) FROM tblusers WHERE username = @usrname", sqlConnection);
                    }
                    else
                    {
                        sqlCommand = new("SELECT COUNT(*) FROM tblusers WHERE username = @usrname AND id != @id", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                    }

                    sqlCommand.Parameters.AddWithValue("@usrname", vm.Model.Username);
                    if ((int)sqlCommand.ExecuteScalar() > 0)
                    {
                        Growl.Info("Username exists!");
                        return;
                    }


                    if (string.IsNullOrEmpty(vm.Model.ID))
                    {
                        sqlCommand = new(@"INSERT INTO tblusers (first_name,last_name,address,contact,username, password, role_id)
                                            VALUES (@fname, @lname, @address, @contact, @username, @password, @roleid)", sqlConnection);
                    }
                    else
                    {
                        sqlCommand = new(@"UPDATE tblusers SET first_name = @fname, last_name = @lname, address = @address, contact = @contact,
                                               username = @username, password = @password, role_id = @roleid WHERE id = @id", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                    }
                    sqlCommand.Parameters.AddWithValue("@lname", vm.Model.LastName);
                    sqlCommand.Parameters.AddWithValue("@fname", vm.Model.FirstName);
                    sqlCommand.Parameters.AddWithValue("@address", vm.Model.Address);
                    sqlCommand.Parameters.AddWithValue("@contact", vm.Model.Contact);
                    sqlCommand.Parameters.AddWithValue("@username", vm.Model.Username);
                    sqlCommand.Parameters.AddWithValue("@password", BCrypt.Net.BCrypt.HashPassword(vm.Model.Password));
                    sqlCommand.Parameters.AddWithValue("@roleid", vm.Model.RoleID);
                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        if (string.IsNullOrEmpty(Model.ID))
                            Growl.Success("Account has been added successfully!");
                        else
                            Growl.Success("Account has been updated successfully!");
                        mw?.UpdateAll();
                        WinHelper.AuditActivity(string.IsNullOrEmpty(Model.ID) ? "ADDED" : "UPDATED", "ACCOUNT");
                        WinHelper.CloseDialog(_btn);
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
