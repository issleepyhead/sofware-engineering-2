using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Windows.Media;
using wrcaysalesinventory.Customs.Dialogs;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class DeliveryPanelViewModel
    {
        public RelayCommand<object> OpenDelivery => new(OpenDeliveryDialog);
        private void OpenDeliveryDialog(object obj) => Dialog.Show(new DeliveryCartDialog());
    }
}
