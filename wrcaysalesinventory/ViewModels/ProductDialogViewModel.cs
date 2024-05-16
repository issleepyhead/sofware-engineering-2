using HandyControl.Controls;
using wrcaysalesinventory.Properties.Langs;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;
using wrcaysalesinventory.Services;
using MessageBox = HandyControl.Controls.MessageBox;
using System.Text.RegularExpressions;

namespace wrcaysalesinventory.ViewModels
{
    public class ProductDialogViewModel : BaseViewModel<ProductModel>
    {
        private DataService _dataService;
        private readonly MainWindow mw;
        private Button _btn;
        private bool _allowed;
        private ProductModel _productModel = new();
        private string _pNameError;
        private string _pCategoryError;
        private string _pPriceError;
        private string _pDescriptionError;
        private string _pCostError;
        private string _pUnitError;
        private Visibility _dVisibility = Visibility.Collapsed;
        private string _criticalLevelError;
        private string _dLabel = Lang.LabelAdd;

        public ProductDialogViewModel(DataService dataService)
        {
            _dataService = dataService;
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        public RelayCommand<object> LoadedCmd => new(LoadedCommand);
        private void LoadedCommand(object obj)
        {
            AllowedDecimal = Model.AllowDecimal;
        }
        
        public Button BTN { get => _btn; set => Set(ref _btn, value); }
        public ObservableCollection<CategoryModel> CategoryDataList => _dataService.GetCategoryPanelList();
        public ObservableCollection<StatusModel> StatusDataList => _dataService.GetStatusList();
        public bool AllowedDecimal { get => _allowed; set => Set(ref _allowed, value); }
        public bool NotAllowed { get => !_allowed; set => Set(ref _allowed, value);  }
        public ProductModel Model { get => _productModel; set {
                Set(ref _productModel, value);
                AllowedDecimal = value.AllowDecimal;
                
                DeleteVisibility = string.IsNullOrEmpty(value.ID) ? Visibility.Collapsed : Visibility.Visible;
                ButtonContent = string.IsNullOrEmpty(value.ID) ?
                    Lang.LabelAdd : (value.StatusName.ToLower() == "inactive" ? Lang.LabelRestore : Lang.LabelUpdate);
            } }
        public string ProductNameError { get => _pNameError; set => Set(ref _pNameError, value); }
        public string ProductCategoryError { get => _pCategoryError; set => Set(ref _pCategoryError, value); }
        public string ProductDescriptionError { get => _pDescriptionError; set => Set(ref _pDescriptionError, value); }
        public string ProductPriceError { get => _pPriceError; set => Set(ref _pPriceError, value); }
        public string ProductCostError { get => _pPriceError; set => Set(ref _pCostError, value); }
        public string ProductUnitError { get => _pUnitError; set => Set(ref _pUnitError, value); }
        public Visibility DeleteVisibility { get => _dVisibility; set => Set(ref _dVisibility, value); }
        public string ButtonContent { get => _dLabel; set => Set(ref _dLabel, value); }
        public string CriticalLevelError { get => _criticalLevelError; set => Set(ref _criticalLevelError, value); }

        public RelayCommand<ProductDialogViewModel> DeleteCmd => new(DeleteCommand);
        public RelayCommand<ProductDialogViewModel> ValidateProduct => new(ValidateModel);

        public RelayCommand<object> CheckedDecimalCmd => new(CheckDecimalCommand);
        private void CheckDecimalCommand(object obj)
        {
            AllowedDecimal = true;
        }

        public RelayCommand<object> UncheckedDecimalCmd => new(UncheckedDecimalCommand);
        private void UncheckedDecimalCommand(object obj)
        {
            AllowedDecimal = false;
        }

        private void DeleteCommand(ProductDialogViewModel vm)
        {
            try
            {
                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                if (vm.Model.StatusName.ToLower() == "Inactive".ToLower())
                {
                    //Dialog.Show("meow");
                    SqlCommand sqlCommand = new(@"DELETE FROM tblproducts WHERE id = @id", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        Growl.Success("Product has been deleted successfully!");
                        WinHelper.AuditActivity("UPDATED", "PRODUCT");
                        WinHelper.CloseDialog(_btn);
                    }
                    else
                        Growl.Info("Failed deleting the product.");
                } else
                {
                    SqlCommand sqlCommand = new("UPDATE tblproducts SET product_status = (SELECT TOP 1 id FROM tblstatus WHERE status_name = 'Inactive') WHERE id = @id", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        Growl.Success("Product name is now inactive!");
                        WinHelper.AuditActivity("UPDATED", "PRODUCT");
                        WinHelper.CloseDialog(_btn);
                    }
                    else
                        Growl.Info("Failed deleting the product.");
                }
                mw?.UpdateAll();   
            }
            catch(SqlException ex)
            {
                if (ex.Message.Contains("DELETE"))
                    MessageBox.Warning(@"This action cannot be completed because the record is referenced by other data in the system. Please remove associated references or contact your system administrator for assistance.", "Unable to delete record.");
                else
                    Growl.Warning("An error occured while performing the action.");
            }
        }      
        private void ValidateModel(ProductDialogViewModel vm)
        {
            // Initialize the validation then validate the model.
            ProductValidator validator = new();
            FluentValidation.Results.ValidationResult result = validator.Validate(vm.Model);

            if (!result.IsValid)
            {
                // Displays error in Error Labels in the product dialog
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

                // List of property of failure.
                List<string> failureproplist = new();
                foreach (var failure in result.Errors)
                    failureproplist.Add(failure.PropertyName);

                // Property list of the ProductModel.
                List<string> proplist = new List<string>
                {
                    nameof(Model.ProductName),
                    nameof(Model.ProductDescription),
                    nameof(vm.Model.ProductPrice),
                    nameof(vm.Model.ProductCost),
                    nameof(vm.Model.ProductUnit)
                };

                // Clear the fields that has been set.
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
            else // If the result of validation is success,
            {
                // clear all the errors displayed in the dialog.
                ProductNameError = null;
                ProductDescriptionError = null;
                ProductPriceError = null;
                ProductCostError = null;
                ProductUnitError = null;
                CriticalLevelError = string.Empty;

                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                SqlCommand sqlCommand;
                try
                {
                    if (!string.IsNullOrEmpty(Model.CriticalLevel) && !_allowed && Regex.IsMatch(Model.CriticalLevel, @"[\p{L}\p{P}\s]"))
                    {
                        CriticalLevelError = "Please provide a valid  value.";
                        return;
                    }

                    // check if the price is less than the cost.
                    if (double.Parse(vm.Model.ProductPrice) <= double.Parse(vm.Model.ProductCost))
                    {
                        ProductPriceError = "Can't be less than or equal to cost.";
                        return;
                    }

                    // Restore the product if it is inactive instead of updating it.
                    if (!string.IsNullOrEmpty(Model.ID) && Model.StatusName == "Inactive")
                    {
                        sqlCommand = new("UPDATE tblproducts SET product_status = (SELECT TOP 1 id FROM tblstatus WHERE status_name = 'Active') WHERE id = @id;", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                        if ((int)sqlCommand.ExecuteNonQuery() > 0)
                        {
                            Growl.Success("Product restored successfully.");
                            mw?.UpdateAll();
                            WinHelper.AuditActivity(string.IsNullOrEmpty(Model.ID) ? "ADDED" : "UPDATED", "PRODUCT");
                            WinHelper.CloseDialog(_btn);
                            return;
                        }
                    }

                    // Check if the product exists.
                    if (string.IsNullOrEmpty(vm.Model.ID))
                    {
                        sqlCommand = new("SELECT COUNT(*) FROM tblproducts WHERE LOWER(product_name) = LOWER(@product_name);", sqlConnection);
                    }
                    else
                    {
                        sqlCommand = new("SELECT COUNT(*) FROM tblproducts WHERE LOWER(product_name) = LOWER(@product_name) AND id != @id;", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                    }
                    sqlCommand.Parameters.AddWithValue("@product_name", vm.Model.ProductName);
                    if ((int)sqlCommand.ExecuteScalar() > 0)
                    {
                        Growl.Info("Product exists!");
                        return;
                    }

                    if (string.IsNullOrEmpty(vm.Model.ID))
                    {
                        sqlCommand = new(@"INSERT INTO tblproducts (product_name, product_description, product_price, product_cost, product_unit, selling_unit, category_id,critical_level)
                                            VALUES (@pname, @pdesc, @pprice, @pcost, @punit, @sunit, @cid, @clevel)", sqlConnection);
                    }
                    else
                    {
                        sqlCommand = new(@"UPDATE tblproducts SET product_name = @pname, product_description = @pdesc, product_price = @pprice, product_cost = @pcost,
                                               product_unit = @punit, selling_unit = @sunit, date_updated = getdate(), category_id = @cid, critical_level = @clevel WHERE id = @id", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                    }
                    sqlCommand.Parameters.AddWithValue("@pname", WinHelper.CapitalizeData(vm.Model.ProductName));
                    sqlCommand.Parameters.AddWithValue("@pdesc", string.IsNullOrEmpty(vm.Model.ProductDescription) ? DBNull.Value : WinHelper.CapitalizeData(vm.Model.ProductDescription));
                    sqlCommand.Parameters.AddWithValue("@pprice", vm.Model.ProductPrice);
                    sqlCommand.Parameters.AddWithValue("@pcost", vm.Model.ProductCost);
                    sqlCommand.Parameters.AddWithValue("@punit", vm.Model.ProductUnit);
                    sqlCommand.Parameters.AddWithValue("@cid", string.IsNullOrEmpty(vm.Model.CategoryID) ? DBNull.Value : vm.Model.CategoryID);
                    sqlCommand.Parameters.AddWithValue("@sunit", _allowed);
                    sqlCommand.Parameters.AddWithValue("@clevel", Model.CriticalLevel);
                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        if (string.IsNullOrEmpty(Model.ID))
                            Growl.Success("Product has been added successfully!");
                        else
                            Growl.Success("Product has been updated successfully!");
                        mw?.UpdateAll();
                        WinHelper.AuditActivity(string.IsNullOrEmpty(Model.ID) ? "ADDED" : "UPDATED", "PRODUCT");
                        WinHelper.CloseDialog(_btn);
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
