using System.Data.SqlClient;
using System.Diagnostics;
using System;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;
using HandyControl.Tools.Command;
using FluentValidation.Results;

namespace wrcaysalesinventory.ViewModels
{
    public class ProductDialogViewModel : BaseViewModel<ProductModel>
    {
        public ProductModel Model { get; set; }

        private string _pNameError;
        public string ProductNameError { get => _pNameError; set => Set(ref _pNameError, value); }

        private string _pDescriptionError;
        public string ProductDescriptionError { get => _pDescriptionError; set => Set(ref _pNameError, value); }

        private string _pPriceError;
        public string ProductPriceError { get => _pPriceError; set => Set(ref _pPriceError, value); }

        private string _pCostError;
        public string ProductCostError { get => _pPriceError; set => Set(ref _pCostError, value); }

        private string _pUnitError;
        public string ProductUnitError { get => _pUnitError; set => Set(ref _pUnitError, value); }

        public RelayCommand<ProductDialogViewModel> ValidateCategory => new(ValidateModel);
        private void ValidateModel(ProductDialogViewModel categoryModel)
        {
            ProductValidator validator = new();
            ValidationResult result = validator.Validate(categoryModel.Model);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.PropertyName)
                    {
                        case nameof(Model.ProductName):
                            _pNameError = failure.ErrorMessage;
                            break;
                        case nameof(Model.ProductDescription):
                            _pDescriptionError = failure.ErrorMessage;
                            break;
                        case nameof(Model.ProductPrice):
                            _pPriceError = failure.ErrorMessage;
                            break;
                        case nameof(Model.ProductCost):
                            _pCostError = failure.ErrorMessage;
                            break;
                        case nameof(Model.ProductUnit):
                            _pUnitError = failure.ErrorMessage;
                            break;
                    }
                }
            }
            else
            {

                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                try
                {

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
