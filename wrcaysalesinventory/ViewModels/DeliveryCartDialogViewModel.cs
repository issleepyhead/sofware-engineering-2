using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
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
        private MainWindow mw;
        public DeliveryCartDialogViewModel(DataService dataService)
        {
            _dataService = dataService;
            DataList = _dataService.GetProductList();
            SupplierList = _dataService.GetSupplierList();
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            deliveryCartModels = new();
        }

        private Button _btn;
        public Button BTN { get => _btn; set=> Set(ref _btn, value); }

        private DeliveryModel _model = new();
        public DeliveryModel Model { get => _model; set => Set(ref _model, value); }

        private ObservableCollection<ProductCartItem> deliveryCartModels = new();
        public ObservableCollection<ProductCartItem> DeliveryCartList { get => deliveryCartModels; set => Set(ref deliveryCartModels, value);}

        public ObservableCollection<SupplierModel> supplierList = new();
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
                    if (pCartModel.ProductID == pModel.ID)
                    {
                        pexists = true;
                        break;
                    }
                }

                if (!pexists)
                {
                    DeliveryCartModel deliveryCartModel = new()
                    {
                        ProductID = pModel.ID,
                        Cost = pModel.ProductCost,
                        ProductName = pModel.ProductName,
                        Quantity = "1"
                    };
                    ProductCartItem pitem = new(deliveryCartModel);
                    pitem.Padding = new Thickness(0);
                    pitem.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    deliveryCartModels.Add(pitem);
                }
            }
        }

        public RelayCommand AddInventory => new(InsertInventory);
        private void InsertInventory()
        {
            SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            SqlCommand sqlCommand;
            try
            {
                double totalItems = 0d;
                double totalCost = 0d;

                foreach (ProductCartItem pitem in deliveryCartModels)
                {
                    DeliveryCartModel dModel = ((DeliveryCartModel)pitem.DataContext);
                    totalCost += double.Parse(dModel.Total);
                    totalItems += double.Parse(dModel.Quantity);
                }
                Model.DueTotal = (totalCost + double.Parse(Model.AdditionalFee)).ToString();


                sqlCommand = new(@"INSERT INTO tbldeliveryheaders (
                                       supplier_id, user_id, invoice_number, additional_fee,
                                       due_total, note
                                    ) VALUES (
                                        @supid, @uid, @in, @af, @dt, @n
                                    )", sqlConnection, sqlTransaction);
                sqlCommand.Parameters.AddWithValue("@supid", Model.SupplierID);
                sqlCommand.Parameters.AddWithValue("@uid", 1);
                sqlCommand.Parameters.AddWithValue("@in", Model.ReferenceNumber);
                sqlCommand.Parameters.AddWithValue("@af", Model.AdditionalFee);
                sqlCommand.Parameters.AddWithValue("@dt", Model.DueTotal);
                sqlCommand.Parameters.AddWithValue("@n", string.IsNullOrEmpty(Model.Note) ? DBNull.Value : Model.Note);
                if(sqlCommand.ExecuteNonQuery() > 0)
                {
                    foreach(ProductCartItem pitem in deliveryCartModels)
                    {
                        DeliveryCartModel dModel = ((DeliveryCartModel)pitem.DataContext);
                        sqlCommand = new(@"INSERT INTO tbldeliveryproducts VALUES (
                                            (SELECT TOP 1 id FROM tbldeliveryheaders ORDER BY id DESC), 
                                            @pid,
                                            @quantity
                                         )", sqlConnection, sqlTransaction);
                        sqlCommand.Parameters.AddWithValue("@pid", dModel.ProductID);
                        sqlCommand.Parameters.AddWithValue("@quantity", dModel.Quantity);
                        if(sqlCommand.ExecuteNonQuery() == 0)
                        {
                            throw new Exception();
                        }
                    }

                    foreach(ProductCartItem pitem in deliveryCartModels)
                    {
                        DeliveryCartModel dModel = ((DeliveryCartModel)pitem.DataContext);
                        sqlCommand = new(@"SELECT COUNT(*) FROM tblinventory WHERE product_id = @pid", sqlConnection, sqlTransaction);
                        sqlCommand.Parameters.AddWithValue("@pid", dModel.ProductID);
                        if((int)sqlCommand.ExecuteScalar() > 0)
                        {
                            sqlCommand = new("UPDATE tblinventory SET stocks = stocks + @q WHERE product_id = @pid",sqlConnection, sqlTransaction);
                        } else
                        {
                            sqlCommand = new("INSERT INTO tblinventory (product_id, stocks) VALUES(@pid, @q)", sqlConnection, sqlTransaction);
                        }
                        sqlCommand.Parameters.AddWithValue("@q", dModel.Quantity);
                        sqlCommand.Parameters.AddWithValue("@pid", dModel.ProductID);
                        if(sqlCommand.ExecuteNonQuery() == 0)
                        {
                            throw new Exception();
                        }
                    }

                    sqlTransaction.Commit();
                    Growl.Success("Delivery has been added to the inventory.");
                    mw.UpdateAll();
                    WinHelper.CloseDialog(_btn);
                } else
                {
                    throw new Exception();
                }

                
            } catch
            {
                sqlTransaction.Rollback();
                Growl.Warning("An error occured while performing the action");
            } 

            

        }
    }
}
