using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class ReceiptViewModel : ViewModelBase
    {
        private TransactionHeaderModel _headerModel;
        public TransactionHeaderModel Header { get => _headerModel; set => Set(ref _headerModel, value); }

        private string _subtotal;
        public string SubTotal { get => _subtotal; set => Set(ref _subtotal, value); }

        private string _cashierName = GlobalData.Config.FullName;
        public string CashierName { get => _cashierName; set => Set(ref _cashierName, value); }

        private string _amountReceived;
        public string AmountReceived { get => _amountReceived; set => Set(ref _amountReceived, value); }
    }
}
