using FluentValidation.Results;
using GalaSoft.MvvmLight;
using HandyControl.Controls;
using HandyControl.Tools.Command;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;

namespace wrcaysalesinventory.ViewModels
{
    public class CategoryDialogViewModel : ViewModelBase
    {
        private CategoryModel _model;
        public CategoryModel Model { get => _model; set => Set(ref _model, value); }

        private string _errName;
        public string CategoryNameError { get => _errName; set => Set(ref _errName, value); }

        private string _errDescription;
        public string DescriptionError { get => _errDescription; set => Set(ref _errDescription, value); }

        public RelayCommand<CategoryDialogViewModel> ValidateCategory => new(ValidateModel);
        private void ValidateModel(CategoryDialogViewModel categoryModel)
        {
            CategoryValidator validator = new();
            ValidationResult result = validator.Validate(categoryModel.Model);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    switch (failure.PropertyName)
                    {
                        case "CategoryName":
                            CategoryNameError = failure.ErrorMessage;
                            break;
                        case "CategoryDescription":
                            DescriptionError = failure.ErrorMessage;
                            break;
                    }
                }
            } else
            {
                CategoryNameError = null;
                DescriptionError = null;
                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                try
                {
                    SqlCommand sqlCommand;
                    if (string.IsNullOrEmpty(Model.ID))
                    {
                        sqlCommand = new("INSERT INTO tblcategories (category_name, category_description) VALUES (@cname, @cdescript)", sqlConnection);
                    } else
                    {
                        sqlCommand = new("UPDATE tblcategories SET category_name = @cname, category_description = @cdescript WHERE id = @id", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", Model.ID);
                    }
                    sqlCommand.Parameters.AddWithValue("@cname", Model.CategoryName);
                    sqlCommand.Parameters.AddWithValue("@cdescript", string.IsNullOrEmpty(Model.CategoryDescription) ? DBNull.Value : Model.CategoryDescription);
                    sqlCommand.ExecuteNonQuery();
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
