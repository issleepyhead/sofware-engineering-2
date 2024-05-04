using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;
using ComboBox = HandyControl.Controls.ComboBox;

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
        public ObservableCollection<StatusModel> StatusDataList { get => _dataService.GetStatusList(); }

        public RelayCommand<object> OpenSupplier => new(OpenSupplierDialog);
        private void OpenSupplierDialog(object obj)
        {
            var d = new SupplierDialog();
            ((SupplierDialogViewModel)d.DataContext).BTN = d.Closebtn;
            ((SupplierDialogViewModel)d.DataContext).Model = new();
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
                    var d = new SupplierDialog();
                    ((SupplierDialogViewModel)d.DataContext).BTN = d.Closebtn;
                    ((SupplierDialogViewModel)d.DataContext).Model = model;
                    Dialog.Show(d);
                    pdataGrid.SelectedItems.Clear();
                    pdataGrid.SelectedCells.Clear();
                }
            }
        }


        public RelayCommand<object> FilterStatusCmd => new(FilterStatusCommand);
        private void FilterStatusCommand(object obj)
        { 
            try
            {
                ComboBox cmbBox = (ComboBox)obj;
                if(cmbBox.SelectedIndex != -1 && cmbBox.SelectedValue.ToString() != "-1")
                {
                    _alldata = _dataService.GetSupplierByStatusList(cmbBox.SelectedValue.ToString());
                    DataList = new ObservableCollection<SupplierModel>(_alldata.Take(30).ToList());
                } else
                {
                    _alldata = _dataService.GetSupplierList();
                    DataList = new ObservableCollection<SupplierModel>(_alldata.Take(30).ToList());
                }
            } catch
            {

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
