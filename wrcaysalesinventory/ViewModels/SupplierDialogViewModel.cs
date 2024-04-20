﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Data.SqlClient;
using System.Diagnostics;
using System;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Windows.Documents;

namespace wrcaysalesinventory.ViewModels
{
    public class SupplierDialogViewModel : ViewModelBase
    {
        public SupplierModel Model { get; set; } = new();

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

        public RelayCommand<SupplierDialogViewModel> ValidateVM => new(ValidateModel);
        private void ValidateModel(SupplierDialogViewModel vm)
        {
            SupplierValidator validator = new();
            ValidationResult result = validator.Validate(vm.Model);
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
                    "SupplierName", "LastName", "FirstName", "Country", "City", "Address", "PhoneNumber"
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
                        sqlCommand = new("UPDATE tblcategories SET category_name = @cname, category_description = @cdescript WHERE id = @id", sqlConnection);
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
