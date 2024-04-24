using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Windows.Controls;
using System.Windows.Forms;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;
using DataGrid = System.Windows.Controls.DataGrid;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class CategoryPanelViewModel : BaseViewModel<CategoryModel>
    {
        private DataService _dataService;
        public CategoryPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            DataList = _dataService.GetGategoryPanelList();
        }

        public RelayCommand<object> OpenCategory => new(OpenCategoryDialog);

        private void OpenCategoryDialog(object obj)
        {
            CategoryDialog d = new CategoryDialog();
            ((CategoryDialogViewModel)d.DataContext).BTN = d.CloseBtn;
            Dialog.Show(d);
        }

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
                    CategoryDialog d = new CategoryDialog(model);
                    ((CategoryDialogViewModel)d.DataContext).BTN = d.CloseBtn;
                    Dialog.Show(d);
                }
            }
        }

        public RelayCommand<SearchBar> SearchCmd => new(SearchCommand);
        public void SearchCommand(SearchBar searchBar)
        {
            DataList = _dataService.SearchGategoryPanelList(string.IsNullOrEmpty(searchBar.Text) ? "%" : searchBar.Text);
        }
    }
}
