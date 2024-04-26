using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class StocksPanelViewModel : BaseViewModel<StocksModel>
    {
        private readonly DataService _dataService;
        public StocksPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            DataList = _dataService.GetStocksList();
        }
    }
}
