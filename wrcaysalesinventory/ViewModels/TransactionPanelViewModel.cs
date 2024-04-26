using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels
{
    public class TransactionPanelViewModel : BaseViewModel<TransactionModel>
    {
        private DataService _dataService;
        public TransactionPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            DataList = _dataService.GetTransactionList();
        }
    }
}
