using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels
{
    public class DeliveryCartDialogViewModel : BaseViewModel<ProductModel>
    {
        private DataService _dataService;
        public DeliveryCartDialogViewModel(DataService dataService)
        {
            _dataService = dataService;
            DataList = _dataService.GetProductList();
            SupplierList = _dataService.GetSupplierList();
        }

        private ObservableCollection<ProductCartItem> deliveryCartModels = [];
        public ObservableCollection<ProductCartItem> DeliveryCartList { get => deliveryCartModels; set => Set(ref deliveryCartModels, value);}

        public ObservableCollection<SupplierModel> supplierList = [];
        public ObservableCollection<SupplierModel> SupplierList { get => supplierList; set => Set(ref supplierList, value);}    

        private string _searchQuery;
        public string SearchQuery { get => _searchQuery; set => Set(ref _searchQuery, value); }

        public RelayCommand<SearchBar> SearchCommand => new(SearchProduct);
        private void SearchProduct(SearchBar searchBar)
        {
            DataList = _dataService.SearchProductList(searchBar.Text);
        }

        public RelayCommand<DataGrid> SelectedCommand => new(AddToCart);
        private void AddToCart(DataGrid obj)
        {
            if(obj.SelectedItems.Count > 0)
            {
                ProductModel pModel = (ProductModel)obj.SelectedItem;
                bool pexists = false;
                for(int i = 0; i < deliveryCartModels.Count; i++)
                {
                    DeliveryCartModel pCartModel = (DeliveryCartModel)deliveryCartModels[i].DataContext;
                    if (pCartModel.ID == pModel.ID)
                    {
                        pexists = true;
                        break;
                    }
                }

                if (!pexists)
                {
                    DeliveryCartModel deliveryCartModel = new DeliveryCartModel
                    {
                        ID = pModel.ID,
                        Cost = pModel.ProductCost,
                        ProductName = pModel.ProductName,
                        Quantity = "1"
                    };
                    ProductCartItem pitem = new(deliveryCartModel);
                    pitem.Padding = new Thickness(0);
                    pitem.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    deliveryCartModel.Total = (double.Parse(deliveryCartModel.Quantity) * double.Parse(deliveryCartModel.Cost)).ToString();
                    deliveryCartModels.Add(pitem);
                }
            }
        }

        public RelayCommand AddInventory => new(InsertInventory);
        private void InsertInventory()
        {
            SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();

        }
    }
}
