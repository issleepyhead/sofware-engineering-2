using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Windows.Media;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class DeliveryPanelViewModel : BaseViewModel<DeliveryModel>
    {
        private DataService _dataService;
        public DeliveryPanelViewModel(DataService dataSevice)
        {
            _dataService = dataSevice;  
            DataList = _dataService.GetDeliveryList();
        }

        public RelayCommand<object> OpenDelivery => new(OpenDeliveryDialog);
        private void OpenDeliveryDialog(object obj)
        {
            DeliveryCartDialog d = new();
            ((DeliveryCartDialogViewModel)d.DataContext).BTN = d.CloseBtn;
            Dialog.Show(d);
        }
    }
}
