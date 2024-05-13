using GalaSoft.MvvmLight;
using HandyControl.Controls;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using wrcaysalesinventory.Data.Classes;
using Button = System.Windows.Controls.Button;

namespace wrcaysalesinventory.ViewModels
{
    public class POSSettingsReceiptDialogViewModel : ViewModelBase
    {
        private string _storeNameData = GlobalData.Config.TransactionReceiptData["store_name"];
        private string _phoneData = GlobalData.Config.TransactionReceiptData["phone"];
        private string _emailData = GlobalData.Config.TransactionReceiptData["email"];
        private string _cashierData = GlobalData.Config.TransactionReceiptData["cashier"];
        private string _addressData = GlobalData.Config.TransactionReceiptData["address"];

        private bool _storeNameField = GlobalData.Config.TransactionReceiptFields["store_name"];
        private bool _phoneField = GlobalData.Config.TransactionReceiptFields["phone"];
        private bool _emailField = GlobalData.Config.TransactionReceiptFields["email"];
        private bool _cashierField = GlobalData.Config.TransactionReceiptFields["cashier"];
        private bool _noteField = GlobalData.Config.TransactionReceiptFields["note"];
        private bool _addressField = GlobalData.Config.TransactionReceiptFields["address"];

        public string StoreNameData { get => _storeNameData; set => Set(ref _storeNameData, value); }
        public string PhoneData { get => _phoneData; set => Set(ref _phoneData, value); }
        public string EmailData { get => _emailData; set => Set(ref _emailData, value); }
        public string CashierData { get => _cashierData; set => Set(ref _cashierData, value); }
        public string AddressData { get => _addressData; set => Set(ref _addressData, value); }

        public bool StoreNameField { get => _storeNameField; set => Set(ref _storeNameField, value); }
        public bool PhoneField { get => _phoneField; set => Set(ref _phoneField, value); }
        public bool EmailField { get => _emailField; set => Set(ref _emailField, value); }
        public bool CashierField { get => _cashierField; set => Set(ref _cashierField, value); }
        public bool NoteField { get => _noteField; set => Set(ref _noteField, value); }
        public bool AddressField { get => _addressField; set => Set(ref _addressField, value); }

        public RelayCommand<object> SaveCmd => new(SaveCommand);
        private void SaveCommand(object obj)
        {
            GlobalData.Config.TransactionReceiptData["store_name"] = StoreNameData;
            GlobalData.Config.TransactionReceiptData["phone"] = PhoneData;
            GlobalData.Config.TransactionReceiptData["email"] = EmailData;
            GlobalData.Config.TransactionReceiptData["address"] = AddressData;
            GlobalData.Config.TransactionReceiptData["cashier"] = CashierData;

            GlobalData.Config.TransactionReceiptFields["store_name"] = StoreNameField;
            GlobalData.Config.TransactionReceiptFields["phone"] = PhoneField;
            GlobalData.Config.TransactionReceiptFields["email"] = EmailField;
            GlobalData.Config.TransactionReceiptFields["address"] = AddressField;
            GlobalData.Config.TransactionReceiptFields["cashier"] = CashierField;
            GlobalData.Config.TransactionReceiptFields["note"] = CashierField;
            GlobalData.Save();
            Growl.Success("Saved Successfully.");
            WinHelper.CloseDialog((Button)obj);
        }

    }
}
