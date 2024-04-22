using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Windows.Controls;
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

        public RelayCommand<object> SelectedCommand => new(SelectionChanged);
        private void SelectionChanged(object obj)
        {
            DataGrid pdataGrid;
            if (obj.GetType() == typeof(DataGrid))
            {
                pdataGrid = (DataGrid)obj;
                if (pdataGrid.SelectedItems.Count > 0)
                {
                    CategoryModel model = (CategoryModel)pdataGrid.SelectedItem;
                    var d = new CategoryDialog(model);
                    ((CategoryDialogViewModel)d.DataContext).BTN = d.CloseBtn;
                    Dialog.Show(d);
                    pdataGrid.ItemsSource = pdataGrid.ItemsSource;
                }
            }
        }
    }
}
