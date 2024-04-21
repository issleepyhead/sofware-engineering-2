using FluentValidation.Results;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Tools.Command;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Windows;
using System.Windows.Documents;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels
{
    public class ProductDialogViewModel : BaseViewModel<ProductModel>
    {
        private DataService _dataService;
        public ProductDialogViewModel(DataService dataService)
        {
            _dataService = dataService;
        }

        public ObservableCollection<CategoryModel> CategoryDataList => _dataService.GetGategoryList();
        public ObservableCollection<StatusModel> StatusDataList => _dataService.GetStatusList();

        private Visibility _statusVisibility = Visibility.Collapsed;
        public Visibility StatusVisibility { get => string.IsNullOrEmpty(Model.ID) ? Visibility.Collapsed : Visibility.Collapsed; set => Set(ref _statusVisibility, value); }

        private ProductModel _productModel = new();
        public ProductModel Model { get => _productModel; set => Set(ref _productModel, value); }

        private bool _allowedDecimal = false;
        public bool IsDecimalAllowed { get => !Model.AllowDecimal; set { Model.AllowDecimal = !_allowedDecimal; Set(ref _allowedDecimal, value); } }
        public bool IsNotDecimalAllowed { get => Model.AllowDecimal; set { Model.AllowDecimal = _allowedDecimal; Set(ref _allowedDecimal, value); } }

        private string _pNameError;
        public string ProductNameError { get => _pNameError; set => Set(ref _pNameError, value); }

        private string _pDescriptionError;
        public string ProductDescriptionError { get => _pDescriptionError; set => Set(ref _pDescriptionError, value); }

        private string _pPriceError;
        public string ProductPriceError { get => _pPriceError; set => Set(ref _pPriceError, value); }

        private string _pCostError;
        public string ProductCostError { get => _pPriceError; set => Set(ref _pCostError, value); }

        private string _pUnitError;
        public string ProductUnitError { get => _pUnitError; set => Set(ref _pUnitError, value); }

        public RelayCommand<ProductDialogViewModel> ValidateProduct => new(ValidateModel);
        private void ValidateModel(ProductDialogViewModel vm)
        {
            ProductValidator validator = new();
            ValidationResult result = validator.Validate(vm.Model);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.PropertyName)
                    {
                        case nameof(Model.ProductName):
                            ProductNameError = failure.ErrorMessage;
                            break;
                        case nameof(vm.Model.ProductDescription):
                            ProductDescriptionError = failure.ErrorMessage;
                            break;
                        case nameof(vm.Model.ProductPrice):
                            ProductPriceError = failure.ErrorMessage;
                            break;
                        case nameof(vm.Model.ProductCost):
                            ProductCostError = failure.ErrorMessage;
                            break;
                        case nameof(vm.Model.ProductUnit):
                            ProductUnitError = failure.ErrorMessage;
                            break;
                    }
                }

                List<string> failureproplist = new();
                foreach (var failure in result.Errors)
                    failureproplist.Add(failure.PropertyName);

                List<string> proplist = new List<string>
                {
                    "ProductName", "ProductDescription", "ProductPrice", "ProductCost", "ProductUnit"
                };
                foreach(var prop in proplist)
                    if (!failureproplist.Contains(prop))
                    {
                        switch (prop)
                        {
                            case nameof(Model.ProductName):
                                ProductNameError = null;
                                break;
                            case nameof(vm.Model.ProductDescription):
                                ProductDescriptionError = null;
                                break;
                            case nameof(vm.Model.ProductPrice):
                                ProductPriceError = null;
                                break;
                            case nameof(vm.Model.ProductCost):
                                ProductCostError = null;
                                break;
                            case nameof(vm.Model.ProductUnit):
                                ProductUnitError = null;
                                break;
                        }
                    }
            }
            else
            {
                ProductNameError = null;
                ProductDescriptionError = null;
                ProductPriceError = null;
                ProductCostError = null;
                ProductUnitError = null;

                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                SqlCommand sqlCommand;
                try
                {
                    if (string.IsNullOrEmpty(vm.Model.ID))
                    {
                        sqlCommand = new(@"INSERT INTO tblproducts (product_name, product_description, product_price, product_cost, product_unit, selling_unit, category_id)
                                            VALUES (@pname, @pdesc, @pprice, @pcost, @punit, @sunit, @cid)", sqlConnection);
                    }
                    else
                    {
                        sqlCommand = new(@"UPDATE tblproducts SET product_name = @pname, product_description = @pdesc, product_price = @pprice, product_cost = @pcost,
                                               product_unit = @punit, product_status = @pstatus, selling_unit = @sunit WHERE id = @id", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                        sqlCommand.Parameters.AddWithValue("@pstatus", vm.Model.StatusID);
                    }
                    sqlCommand.Parameters.AddWithValue("@pname", vm.Model.ProductName);
                    sqlCommand.Parameters.AddWithValue("@pdesc", string.IsNullOrEmpty(vm.Model.ProductDescription) ? DBNull.Value : vm.Model.ProductDescription);
                    sqlCommand.Parameters.AddWithValue("@pprice", vm.Model.ProductPrice);
                    sqlCommand.Parameters.AddWithValue("@pcost", vm.Model.ProductCost);
                    sqlCommand.Parameters.AddWithValue("@punit", vm.Model.ProductUnit);
                    sqlCommand.Parameters.AddWithValue("@cid", vm.Model.CategoryID);
                    sqlCommand.Parameters.AddWithValue("@sunit", vm.Model.AllowDecimal);
                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        if (string.IsNullOrEmpty(Model.ID))
                            Growl.Success("Product has been added successfully!");
                        else
                            Growl.Success("Product has been updated successfully!");
                    }
                    else
                    {
                        Growl.Info("An error occured while performing an action.");
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
