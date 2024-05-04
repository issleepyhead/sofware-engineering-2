using HandyControl.Controls;
using HandyControl.Tools.Command;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using wrcaysalesinventory.Properties.Langs;
using System.Windows.Input;
using wrcaysalesinventory.Data.Classes;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class ProductPanelViewModel : BaseViewModel<ProductModel>, IUpdateData
    {
        private DataService _dataService;
        private ObservableCollection<ProductModel> _alldata;
        private ObservableCollection<CategoryModel> _categories;
        public ProductPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            _alldata = _dataService.GetProductList();
            DataList = new ObservableCollection<ProductModel>(_alldata.Take(30).ToList());
            _categories = _dataService.GetCategoryListComboBox();
        }

        public void UpdateData()
        {
            _alldata = _dataService.GetProductList();
            DataList = new ObservableCollection<ProductModel>(_alldata.Take(30).ToList());
            CategoryDataList = _dataService.GetCategoryListComboBox();
        }

        public ObservableCollection<CategoryModel> CategoryDataList { get => _categories; set => Set(ref _categories, value); }
        public ObservableCollection<StatusModel> StatusDataList { get => _dataService.GetStatusList(); }

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

        public RelayCommand<object> RefreshCmd=> new(RefreshCommand);

        private void RefreshCommand(object obj)
        {
            _alldata = _dataService.GetProductList();
            DataList = new ObservableCollection<ProductModel>(_alldata.Take(30).ToList());
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
                    ProductDialog d = new ProductDialog(model);
                    ((ProductDialogViewModel)d.DataContext).BTN = d.CloseBtn;
                    ((ProductDialogViewModel)d.DataContext).Model = model;
                    Dialog.Show(d);
                    pdataGrid.SelectedItems.Clear();
                    pdataGrid.SelectedCells.Clear();
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
