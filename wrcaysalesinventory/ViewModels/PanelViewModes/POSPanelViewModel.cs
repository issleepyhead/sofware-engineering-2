using HandyControl.Controls;
using HandyControl.Tools.Command;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
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
            CustomerList = _dataService.GetCustomerList();
        }

        private ObservableCollection<POSCartModel> _data = [];
        public ObservableCollection<POSCartModel> CartList { get => _data; set => Set(ref _data, value); }
        private ObservableCollection<CustomerModel> _customerList;
        public ObservableCollection<CustomerModel> CustomerList { get => _customerList; set => Set(ref _customerList, value); }
        private TransactionHeaderModel _header = new();
        public TransactionHeaderModel Header { get => _header; set => Set(ref _header, value); }

        private string _subtotal;
        public string SubTotal { get => _subtotal; set => Set(ref _subtotal, value); }
        private string _totalAmount = "0";

        private string _discount = "0";

        public string Discount { get {
                if (Regex.IsMatch(_discount, "^(\\d+)?\\.?(\\d+)$"))
                {
                    return _discount;
                }
                else
                {
                    return null;
                }
            } set {  Set(ref _discount, value); ValueChanged(); } }
        private string _additional = "0";
        public string AdditionalFee { get
            {
                if (Regex.IsMatch(_additional, "^(\\d+)?\\.?(\\d+)$"))
                {
                    return _additional;
                } else
                {
                    return null;
                }
            } set
            {
                Set(ref _additional, value);
                ValueChanged();
            }
        }
        private string _vat = "0";
        public string VAT { get => _vat; set => Set(ref _vat, value); }
        public string TotalAmount { get => _totalAmount; set { Set(ref _totalAmount, value); } }


        public RelayCommand<SearchBar> SearchCommand => new(SearchProduct);
        private void SearchProduct(SearchBar searchBar)
        {
            DataList = _dataService.SearchStocksList(searchBar.Text);
        }

        public void ValueChanged()
        {
            double total = 0;
            foreach (POSCartModel model in CartList)
            {
                total += double.Parse(model.Total);
            }
            SubTotal = total.ToString();
            TotalAmount = (total + double.Parse(AdditionalFee ?? "0") - (total * (double.Parse(Discount ?? "0") / 100)) + double.Parse(VAT)).ToString();
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
                    POSCartModel pCartModel = CartList[i];
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
                    //POSProductItem pitem = new(deliveryCartModel);
                    //pitem.HorizontalAlignment = HorizontalAlignment.Stretch;
                    CartList.Add(deliveryCartModel);
                }

                double total = 0;
                foreach(POSCartModel model in CartList)
                {
                    total += double.Parse(model.Total);
                }
                SubTotal = total.ToString();
                TotalAmount = (total + double.Parse(AdditionalFee ?? "0") - (total * (double.Parse(Discount ?? "0") / 100)) + double.Parse(VAT)).ToString();
            }
        }

        public RelayCommand<object> OpenDiscountCmd => new(OpenDiscount);

        private void OpenDiscount(object obj)
        {
            DiscountDialog d = new(this);
            Dialog.Show(d);
        }

        public RelayCommand<object> OpenAFeeCmd => new(OpenAFee);

        private void OpenAFee(object obj)
        {
            AdditionalFeeDialog d = new(this);
            Dialog.Show(d);
        }

        public RelayCommand<object> OpenCustomerCmd => new(OpenCustomer);

        private void OpenCustomer(object obj)
        {
            CustomerDialog d = new(this);
            Dialog.Show(d);
        }

        public RelayCommand<object> OpenNoteCmd => new(OpenNote);

        private void OpenNote(object obj)
        {
            NoteDialog d = new(this);
            Dialog.Show(d);
        }
    }
}
