using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;

namespace wrcaysalesinventory.ViewModels
{
    public class CustomerDialogViewModel : ViewModelBase
    {
        private string _firstNameError;
        private string _lastNameError;
        private string _phoneError;
        private string _emailError;
        private MainWindow mw;
        public CustomerDialogViewModel()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }
        private Button _btn;
        public Button BTN { get => _btn; set => Set(ref _btn, value); }

        private CustomerModel _model;
        public CustomerModel Model { get => _model; set => Set(ref _model, value); }

        public string EmailError { get => _emailError; set => Set(ref _emailError, value); }
        public string FirstNameError { get => _firstNameError; set => Set(ref _firstNameError, value); }
        public string LastNameError { get => _lastNameError; set => Set(ref _lastNameError, value); }
        public string PhoneError { get => _phoneError; set => Set(ref _phoneError, value); }

        public RelayCommand<CustomerModel> AddCustomer => new(AddCustomerCmd);
        private void AddCustomerCmd(CustomerModel obj)
        {
            CustomerValidator validator = new();
            FluentValidation.Results.ValidationResult result = validator.Validate(Model);
            if(!result.IsValid)
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
                        case nameof(Model.Email):
                            EmailError = failure.ErrorMessage;
                            break;
                        case nameof(Model.Phone):
                            PhoneError = failure.ErrorMessage;
                            break;
                    }
                }
            } else
            {
                FirstNameError = string.Empty;
                LastNameError = string.Empty;
                EmailError = string.Empty;
                PhoneError = string.Empty;
                try
                {
                    SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                    SqlCommand sqlCommand;

                    if(string.IsNullOrEmpty(Model.ID))
                    {
                        sqlCommand = new("INSERT INTO tblcustomers VALUES(@fname, @lname, @phone, @email, @points)", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@points", "0");
                    } else
                    {
                        sqlCommand = new("UPDATE tblcustomers SET first_name = @fname, last_name = @lname, phone = @phone, email = @email", sqlConnection);
                    }
                    sqlCommand.Parameters.AddWithValue("@fname", Model.FirstName);
                    sqlCommand.Parameters.AddWithValue("@lname", Model.LastName);
                    sqlCommand.Parameters.AddWithValue("@phone", Model.Phone);
                    sqlCommand.Parameters.AddWithValue("@email", Model.Email);
                    if ((int)sqlCommand.ExecuteNonQuery() > 0)
                    {
                        if(string.IsNullOrEmpty(Model.ID))
                        {
                            Growl.Success("Customer has been added successfully!");
                        } else
                        {
                            Growl.Success("Customer has been updated successfully!");
                        }
                        WinHelper.CloseDialog(BTN);
                        mw?.UpdateAll();
                    }
                    
                }
                catch
                {

                }
            }
        }

        public RelayCommand<CustomerModel> DeleteCustomer => new(DeleteCustomerCommand);
        private void DeleteCustomerCommand(CustomerModel customerModel)
        {
            try
            {

            } catch
            {

            }
        }

    }
}
