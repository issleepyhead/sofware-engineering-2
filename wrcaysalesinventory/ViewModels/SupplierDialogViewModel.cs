using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;
using wrcaysalesinventory.Properties.Langs;
using MessageBox = HandyControl.Controls.MessageBox;

namespace wrcaysalesinventory.ViewModels
{
    public class SupplierDialogViewModel : ViewModelBase
    {
        private readonly MainWindow mw;
        private Visibility _dVisibility = Visibility.Collapsed;
        private string _dLabel = Lang.LabelAdd;
        public SupplierDialogViewModel()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        private SupplierModel _model = new();
        public SupplierModel Model { get => _model;
            set 
            {
                Set(ref _model, value);
                DeleteVisibility = string.IsNullOrEmpty(value.ID) ? Visibility.Collapsed : Visibility.Visible;
                ButtonContent = string.IsNullOrEmpty(value.ID) ?
                    Lang.LabelAdd : (value.Status.ToLower() == "inactive" ? Lang.LabelRestore : Lang.LabelUpdate);
            } 
        }

        private Button _btn;
        public Button BTN { get => _btn; set => Set(ref _btn, value); }

        private string _supplierNameError;
        public string SupplierNameError { get => _supplierNameError; set => Set(ref _supplierNameError, value); }

        private string _firstNameError;
        public string FirstNameError { get => _firstNameError; set => Set(ref _firstNameError, value); }

        private string _lastNameError;
        public string LastNameError { get => _lastNameError; set => Set(ref _lastNameError, value); }

        private string _cityError;
        public string CityError { get => _cityError; set => Set(ref _cityError, value); }

        private string _countryError;
        public string CountryError { get => _countryError; set => Set(ref _countryError, value); }

        private string _addressError;
        public string AddressError { get => _addressError; set => Set(ref _addressError, value); }

        private string _phoneError;
        public string PhoneError { get => _phoneError; set => Set(ref _phoneError, value); }

        public Visibility DeleteVisibility { get => _dVisibility; set => Set(ref _dVisibility, value); }
        public string ButtonContent { get => _dLabel; set => Set(ref _dLabel, value); }

        public RelayCommand<SupplierDialogViewModel> DeleteCmd => new(DeleteCommand);
        private void DeleteCommand(SupplierDialogViewModel vm)
        {
            try
            {

                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                SqlCommand sqlCommand;
                if (vm.Model.Status == "Inactive")
                {
                    sqlCommand = new("DELETE FROM tblsuppliers WHERE id = @id", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        Growl.Success("Supplier has been deleted successfully!");
                        WinHelper.AuditActivity("DELETED", "SUPPLIER");
                    }
                    else
                        Growl.Info("Failed deleting the supplier.");
                }
                else
                {
                    sqlCommand = new("UPDATE tblsuppliers SET status_id = (SELECT TOP 1 id FROM tblstatus WHERE status_name = 'Inactive') WHERE id = @id", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        Growl.Success("Supplier has been set to Inactive");
                        WinHelper.AuditActivity("DELETED", "SUPPLIER");
                        
                    }
                    else
                        Growl.Info("Failed deleting the supplier.");
                }
                WinHelper.CloseDialog(_btn);
                mw?.UpdateAll();

            }
            catch(SqlException ex)
            {
                if(ex.Message.Contains("DELETE"))
                    MessageBox.Warning(@"This action cannot be completed because the record is referenced by other data in the system. Please remove associated references or contact your system administrator for assistance.", "Unable to delete record.");
                else
                    Growl.Warning("An error occured while performing the action.");
            }
        }

        public RelayCommand<SupplierDialogViewModel> ValidateVM => new(ValidateModel);
        private void ValidateModel(SupplierDialogViewModel vm)
        {
            SupplierValidator validator = new();
            FluentValidation.Results.ValidationResult result = validator.Validate(vm.Model);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.PropertyName)
                    {
                        case nameof(Model.SupplierName):
                            SupplierNameError = failure.ErrorMessage;
                            break;
                        case nameof(Model.FirstName):
                            FirstNameError = failure.ErrorMessage;
                            break;
                        case nameof(Model.LastName):
                            LastNameError = failure.ErrorMessage;
                            break;
                        case nameof(Model.Country):
                            CountryError = failure.ErrorMessage;
                            break;
                        case nameof(Model.City):
                            CityError = failure.ErrorMessage;
                            break;
                        case nameof(Model.Address):
                            AddressError = failure.ErrorMessage;
                            break;
                        case nameof(Model.PhoneNumber):
                            PhoneError = failure.ErrorMessage;
                            break;
                    }
                }

                List<string> failureproplist = new();
                foreach (var failure in result.Errors)
                    failureproplist.Add(failure.PropertyName);

                List<string> proplist = new List<string>
                {
                    nameof(Model.SupplierName),
                    nameof(Model.LastName),
                    nameof(Model.FirstName),
                    nameof(Model.Country),
                    nameof(Model.City),
                    nameof(Model.Address),
                    nameof(Model.PhoneNumber)
                };

                foreach (var prop in proplist)
                    if (!failureproplist.Contains(prop))
                    {
                        switch (prop)
                        {
                            case nameof(Model.SupplierName):
                                SupplierNameError = null;
                                break;
                            case nameof(Model.FirstName):
                                FirstNameError = null;
                                break;
                            case nameof(Model.LastName):
                                LastNameError = null;
                                break;
                            case nameof(Model.Country):
                                CountryError = null;
                                break;
                            case nameof(Model.City):
                                CityError = null;
                                break;
                            case nameof(Model.Address):
                                AddressError = null;
                                break;
                            case nameof(Model.PhoneNumber):
                                PhoneError = null;
                                break;
                        }
                    }
            }
            else
            {
                SupplierNameError = null;
                FirstNameError = null;
                LastNameError = null;
                CountryError = null;
                CityError = null;
                AddressError = null;
                PhoneError = null;
                try
                {
                    SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                    SqlCommand sqlCommand;

                    if (string.IsNullOrEmpty(vm.Model.ID))
                    {
                        sqlCommand = new("SELECT COUNT(*) FROM tblsuppliers WHERE supplier_name = @suppliername OR phone_number = @phone", sqlConnection);
                    }
                    else
                    {
                        sqlCommand = new("SELECT COUNT(*) FROM tblsuppliers WHERE (supplier_name = @suppliername OR phone_number = @phone) AND id != @id", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                    }
                    sqlCommand.Parameters.AddWithValue("@suppliername", vm.Model.SupplierName);
                    sqlCommand.Parameters.AddWithValue("@phone", vm.Model.PhoneNumber);
                    if ((int)sqlCommand.ExecuteScalar() > 0)
                    {
                        Growl.Info("Supplier exists!");
                        return;
                    }
                    if (string.IsNullOrEmpty(Model.ID))
                    {
                        sqlCommand = new(@"INSERT INTO tblsuppliers (
                                            supplier_name, last_name, first_name, city, country, address, phone_number
                                        ) VALUES (
                                            @sname, @lname, @fname, @scity, @scountry, @saddress, @sphone
                                        )", sqlConnection);
                    }
                    else
                    {
                        sqlCommand = new(@"UPDATE tblsuppliers SET
                                                supplier_name = @sname,
                                                last_name = @lname,
                                                first_name = @fname,
                                                city = @scity,
                                                country = @scountry,
                                                address = @saddress,
                                                phone_number = @sphone
                                        WHERE id = @id", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", Model.ID);
                    }
                    sqlCommand.Parameters.AddWithValue("@sname", Model.SupplierName);
                    sqlCommand.Parameters.AddWithValue("@lname", string.IsNullOrEmpty(Model.FirstName) ? DBNull.Value : Model.FirstName);
                    sqlCommand.Parameters.AddWithValue("@fname", string.IsNullOrEmpty(Model.LastName) ? DBNull.Value : Model.LastName);
                    sqlCommand.Parameters.AddWithValue("@scity", string.IsNullOrEmpty(Model.City) ? DBNull.Value : Model.City);
                    sqlCommand.Parameters.AddWithValue("@scountry", string.IsNullOrEmpty(Model.Country) ? DBNull.Value : Model.Country);
                    sqlCommand.Parameters.AddWithValue("@saddress", string.IsNullOrEmpty(Model.Address) ? DBNull.Value : Model.Address);
                    sqlCommand.Parameters.AddWithValue("@sphone", Model.PhoneNumber);
                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        if (string.IsNullOrEmpty(Model.ID))
                            Growl.Success("Supplier has been added successfully!");
                        else
                            Growl.Success("Supplier has been updated succesfully!");
                        mw?.UpdateAll();
                        WinHelper.AuditActivity(string.IsNullOrEmpty(Model.ID) ? "ADDED" : "UPDATED", "SUPPLIER");
                        WinHelper.CloseDialog(_btn);
                    }
                    else
                    {
                        Growl.Warning("An error occured while performing an action.");
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
