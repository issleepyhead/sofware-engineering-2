using GalaSoft.MvvmLight;
using HandyControl.Controls;
using HandyControl.Tools.Command;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;
using MessageBox = HandyControl.Controls.MessageBox;

namespace wrcaysalesinventory.ViewModels
{
    public class CategoryDialogViewModel : ViewModelBase
    {
        private readonly MainWindow mw;
        public CategoryDialogViewModel()
        {
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        private Button _btn;
        public Button BTN { get => _btn; set => Set(ref _btn, value ); }

        private CategoryModel _model;
        public CategoryModel Model { get => _model; set => Set(ref _model, value); }

        private string _errName;
        public string CategoryNameError { get => _errName; set => Set(ref _errName, value); }

        private string _errDescription;
        public string DescriptionError { get => _errDescription; set => Set(ref _errDescription, value); }

        public RelayCommand<CategoryDialogViewModel> DeleteCmd => new(DeleteCommand);
        private void DeleteCommand(CategoryDialogViewModel vm)
        {
            try
            {
                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                SqlCommand sqlCommand = new("DELETE FROM tblcategories WHERE id = @id", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", vm.Model.ID);
                if (sqlCommand.ExecuteNonQuery() > 0)
                {
                    Growl.Success("Category has been deleted successfully!");
                    mw.UpdateAll();
                    WinHelper.AuditActivity("DELETED", "CATEGORY");
                    WinHelper.CloseDialog(_btn);
                }
                else
                {
                    Growl.Info("Failed deleting the category.");
                }
            } catch(SqlException ex)
            {
                if (ex.Message.Contains("DELETE"))
                    MessageBox.Warning(@"This action cannot be completed because the record is referenced by other data in the system. Please remove associated references or contact your system administrator for assistance.", "Unable to delete record.");
                else
                    Growl.Warning("An error occured while performing the action.");
            }
        }

        public RelayCommand<CategoryDialogViewModel> ValidateCategory => new(ValidateModel);
        private void ValidateModel(CategoryDialogViewModel categoryModel)
        {
            CategoryValidator validator = new();
            FluentValidation.Results.ValidationResult result = validator.Validate(categoryModel.Model);
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
                SqlCommand sqlCommand;
                try
                {
                    if(!string.IsNullOrEmpty(Model.ID))
                    {
                        sqlCommand = new("SELECT COUNT(*) FROM tblcategories WHERE category_name = @cname AND id != @id", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", Model.ID);
                    } else
                    {
                        sqlCommand = new("SELECT COUNT(*) FROM tblcategories WHERE category_name = @cname", sqlConnection);
                    }
                    sqlCommand.Parameters.AddWithValue("@cname", Model.CategoryName);
                    if((int)sqlCommand.ExecuteScalar() > 0)
                    {
                        Growl.Info("Category exists!");
                        return;
                    }
                    

                    if (string.IsNullOrEmpty(Model.ID))
                    {
                        sqlCommand = new("INSERT INTO tblcategories (category_name, category_description) VALUES (@cname, @cdescript)", sqlConnection);
                    } else
                    {
                        sqlCommand = new("UPDATE tblcategories SET category_name = @cname, category_description = @cdescript, date_updated = getdate() WHERE id = @id", sqlConnection);
                        sqlCommand.Parameters.AddWithValue("@id", Model.ID);
                    }
                    sqlCommand.Parameters.AddWithValue("@cname", Model.CategoryName);
                    sqlCommand.Parameters.AddWithValue("@cdescript", string.IsNullOrEmpty(Model.CategoryDescription) ? DBNull.Value : Model.CategoryDescription);
                    if (sqlCommand.ExecuteNonQuery() > 0)
                    {
                        
                        if (string.IsNullOrEmpty(Model.ID))
                            Growl.Success("Category has been added successfully!");
                        else
                            Growl.Success("Category has been updated succesfully!");
                        mw?.UpdateAll();
                        WinHelper.AuditActivity(string.IsNullOrEmpty(Model.ID) ? "ADDED" : "UPDATED", "CATEGORY");
                        WinHelper.CloseDialog(_btn);
                    } else
                    {
                        Growl.Warning("An error occured while performing an action.");
                    }
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }
    }
}
