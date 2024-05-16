using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class DeliveryPanelViewModel : BaseViewModel<DeliveryModel>
    {
        private DataService _dataService;
        private ObservableCollection<DeliveryModel> _alldata;
        public DeliveryPanelViewModel(DataService dataSevice)
        {
            _dataService = dataSevice;
            _alldata = _dataService.GetDeliveryList();
            DataList = new ObservableCollection<DeliveryModel>(_alldata.Take(30).ToList());
        }

        public int TotalData { get => _alldata.Count; }

        public RelayCommand<object> OpenDelivery => new(OpenDeliveryDialog);
        private void OpenDeliveryDialog(object obj)
        {
            DeliveryCartDialog d = new();
            ((DeliveryCartDialogViewModel)d.DataContext).BTN = d.CloseBtn;
            Dialog.Show(d);
        }

        public RelayCommand<string> SearchCmd => new(SearchCommand);
        private void SearchCommand(string query)
        {
            _alldata = _dataService.SearchDeliveryList(string.IsNullOrEmpty(query) ? "%" : query);
            DataList = new ObservableCollection<DeliveryModel>(_alldata.Take(30).ToList());
        }

        public RelayCommand<object> RefreshData => new(R);
        private void R(object query)
        {
            _alldata = _dataService.GetDeliveryList();
            DataList = new ObservableCollection<DeliveryModel>(_alldata.Take(30).ToList());
        }


        public void PageUpdated(int offset)
        {
            DataList = new ObservableCollection<DeliveryModel>(_alldata.Skip(offset * 30).Take(30).ToList());
        }

        public RelayCommand<DataGrid> SelectedCommand => new(SelectionChanged);
        private void SelectionChanged(DataGrid dataGrid)
        {
                if (dataGrid.SelectedItems.Count > 0)
                {
                    DeliveryModel model = (DeliveryModel)dataGrid.SelectedItem;
                    DeliveryDetailsDialog d  = new DeliveryDetailsDialog();
                    ((DeliveryDetailsDialogViewModel)d.DataContext).Model = model;
                    Dialog.Show(d);
                }
        }


    }
}
