using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Classes;

namespace wrcaysalesinventory.ViewModels
{
    public class POSSettingsPanelViewModel : ViewModelBase
    {
        private string _vat = GlobalData.Config.TransactionVAT;
        public string VAT { get => _vat; set => Set(ref _vat, value); }

        private string _quota = GlobalData.Config.TransactionQuota;
        public string Quota { get => _quota; set => Set(ref _quota, value); }

        private bool _isAutoPrint = GlobalData.Config.TransactionPrintReceipt;
        public bool AutoPrintEnable { get => _isAutoPrint; set => Set(ref _isAutoPrint, value); }
        public RelayCommand<object> CheckedAutoPrintCmd => new(CheckedAutoPrintCommand);
        private void CheckedAutoPrintCommand(object obj)
        {
            GlobalData.Config.TransactionPrintReceipt = true;
        }

        public RelayCommand<object> OpenReceiptCmd => new(OpenReceiptCommand);
        private void OpenReceiptCommand(object obj)
        {
            Dialog.Show(new POSSettingsReceiptDialog());
        }

        public RelayCommand<object> UnCheckedAutoPrintCmd => new(CheckedAutoPrintCommand);
        private void UnCheckedAutoPrintCommand(object obj)
        {
            GlobalData.Config.TransactionPrintReceipt = false;
        }


        public RelayCommand<object> SaveCmd => new(SaveCommand);
        private void SaveCommand(object obj)
        {
            GlobalData.Config.TransactionVAT = VAT;
            GlobalData.Config.TransactionQuota = Quota;
            GlobalData.Save();
        }
    }
}
