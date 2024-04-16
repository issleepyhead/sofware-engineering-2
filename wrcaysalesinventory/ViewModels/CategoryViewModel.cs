using FluentValidation.Results;
using HandyControl.Tools.Command;
using System.Security.RightsManagement;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Data.Models.Validations;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels
{
    public class CategoryViewModel : BaseViewModel<CategoryModel>
    {
        public DataService _dataService;

        public CategoryViewModel(DataService dataService)
        {
            _dataService = dataService;
            //DataList = _dataService.GetGategoryList();
        }


    }
}
