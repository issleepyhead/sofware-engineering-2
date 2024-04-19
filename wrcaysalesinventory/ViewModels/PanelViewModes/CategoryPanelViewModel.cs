using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class CategoryPanelViewModel : BaseViewModel<CategoryModel>
    {
        public CategoryPanelViewModel(DataService dataService)
        {
            DataList = dataService.GetGategoryPanelList();
        }

        public RelayCommand<object> OpenCategory => new(OpenCategoryDialog);

        private void OpenCategoryDialog(object obj) => Dialog.Show(new CategoryDialog());
    }
}
