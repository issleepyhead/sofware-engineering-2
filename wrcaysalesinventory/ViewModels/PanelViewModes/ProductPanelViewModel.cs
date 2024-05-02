using HandyControl.Controls;
using HandyControl.Tools.Command;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class ProductPanelViewModel : BaseViewModel<ProductModel>
    {
        private DataService _dataService;
        private ObservableCollection<ProductModel> _alldata;
        public ProductPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            _alldata = _dataService.GetProductList();
            DataList = new ObservableCollection<ProductModel>(_alldata.Take(30).ToList());
        }

        public ObservableCollection<CategoryModel> CategoryDataList { get => _dataService.GetGategoryList(); }

        public int TotalData { get => _alldata.Count; }
        public DataService DataService { get => _dataService; } 
        public ObservableCollection<ProductModel> AllData { set { Set(ref _alldata,value);  DataList = new ObservableCollection<ProductModel>(_alldata.Take(30).ToList()); } }

        public RelayCommand<object> OpenDialog => new(OpenProductDialog);

        private void OpenProductDialog(object obj)
        {
            var d = new ProductDialog();
            ((ProductDialogViewModel)d.DataContext).BTN = d.CloseBtn;
            Dialog.Show(d);
        }

        public RelayCommand<object> SelectedCommand => new(SelectionChanged);
        private void SelectionChanged(object obj)
        {
            DataGrid pdataGrid;
            if (obj.GetType() == typeof(DataGrid))
            {
                 pdataGrid = (DataGrid)obj;
                if(pdataGrid.SelectedItems.Count > 0)
                {
                    ProductModel model = (ProductModel)pdataGrid.SelectedItem;
                    var d = new ProductDialog(model);
                    ((ProductDialogViewModel)d.DataContext).BTN = d.CloseBtn;
                    Dialog.Show(d);
                }
            }
        }

        public RelayCommand<SearchBar> SearchCmd => new(SearchCommand);
        public void SearchCommand(SearchBar searchBar)
        {
            DataList = _dataService.SearchProductList(string.IsNullOrEmpty(searchBar.Text) ? "%" : searchBar.Text);
        }

        public void PageUpdated(int offset)
        {
            DataList = new ObservableCollection<ProductModel>(_alldata.Skip(offset * 30).Take(30).ToList());
        }
    }
}
