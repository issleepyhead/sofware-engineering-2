using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class SupplierPanelViewModel : BaseViewModel<SupplierModel>
    {
        private readonly DataService _dataService;
        private ObservableCollection<SupplierModel> _alldata;
        public SupplierPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            _alldata = _dataService.GetSupplierList();
            DataList = new ObservableCollection<SupplierModel>(_alldata.Take(30).ToList());
        }

        public int TotalData { get => _alldata.Count; }

        public RelayCommand<object> OpenSupplier => new(OpenSupplierDialog);
        private void OpenSupplierDialog(object obj)
        {
            var d = new SupplierDialog();
            ((SupplierDialogViewModel)d.DataContext).BTN = d.Closebtn;
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
                    SupplierModel model = (SupplierModel)pdataGrid.SelectedItem;
                    var d = new SupplierDialog(model);
                    ((SupplierDialogViewModel)d.DataContext).BTN = d.Closebtn;
                    ((SupplierDialogViewModel)d.DataContext).Model = model;
                    Dialog.Show(d);
                }
            }
        }

        public RelayCommand<SearchBar> SearchCmd => new(SearchCommand);
        public void SearchCommand(SearchBar searchBar)
        {
            DataList = _dataService.SearchSupplierList(string.IsNullOrEmpty(searchBar.Text) ? "%" : searchBar.Text);
        }

        public void PageUpdated(int offset)
        {
            DataList = new ObservableCollection<SupplierModel>(_alldata.Skip(offset * 30).Take(30).ToList());
        }
    }
}
