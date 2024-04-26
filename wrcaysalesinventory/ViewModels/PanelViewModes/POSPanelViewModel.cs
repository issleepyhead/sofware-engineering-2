using HandyControl.Controls;
using HandyControl.Tools.Command;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class POSPanelViewModel : BaseViewModel<StocksModel>
    {
        private DataService _dataService;
        public POSPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            DataList = _dataService.GetStocksList();
        }

        private ObservableCollection<POSProductItem> _data = new();
        public ObservableCollection<POSProductItem> CartList { get => _data; set => Set(ref _data, value); }

        public RelayCommand<SearchBar> SearchCommand => new(SearchProduct);
        private void SearchProduct(SearchBar searchBar)
        {
            DataList = _dataService.SearchStocksList(searchBar.Text);
        }

        public RelayCommand<DataGrid> SelectedCommand => new(AddToCart);
        private void AddToCart(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItems.Count > 0)
            {
                StocksModel pModel = (StocksModel)dataGrid.SelectedItem;
                bool pexists = false;
                for (int i = 0; i < CartList.Count; i++)
                {
                    POSCartModel pCartModel = (POSCartModel)CartList[i].DataContext;
                    if (pCartModel.ID == pModel.ID)
                    {
                        pexists = true;
                        break;
                    }
                }

                if (!pexists)
                {
                    POSCartModel deliveryCartModel = new POSCartModel
                    {
                        ID = pModel.ID,
                        Cost = pModel.Cost,
                        ProductName = pModel.ProductName,
                        Quantity = "1"
                    };
                    POSProductItem pitem = new(deliveryCartModel);
                    pitem.HorizontalAlignment = HorizontalAlignment.Stretch;
                    CartList.Add(pitem);
                }
            }
        }
    }
}
