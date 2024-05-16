using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text.RegularExpressions;
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

        private ObservableCollection<DeliveryCartModel> deliveryCartModels = new();
        public ObservableCollection<DeliveryCartModel> DeliveryCartList { get => deliveryCartModels; set => Set(ref deliveryCartModels, value);}

        public ObservableCollection<SupplierModel> supplierList = new();
        public ObservableCollection<SupplierModel> SupplierList { get => supplierList; set => Set(ref supplierList, value);}

        private string _subtotal = "0";
        public string  SubTotal { get => _subtotal; set => Set(ref _subtotal, value); }

        private string _grandtotal = "0";
        public string GrandTotal { get => _grandtotal; set => Set(ref _grandtotal, value); }

        private string _searchQuery;
        public string SearchQuery { get => _searchQuery; set => Set(ref _searchQuery, value); }

        private string _additionalFee;
        public string AdditionalFee { get => _additionalFee; set {
                if (Regex.IsMatch(value,"^(\\d+)?\\.?(\\d+)$"))
                {
                    Set(ref _additionalFee, value);
                    ValueChanged();
                } else
                {
                    Set(ref _additionalFee, "0");
                }
                Model.AdditionalFee = _additionalFee;
            } }

        public void ValueChanged()
        {
            try
            {
                double total = 0;
                foreach (DeliveryCartModel model in deliveryCartModels)
                {
                    total += double.Parse(model.Total);
                }
                SubTotal = total.ToString();
                GrandTotal = (total + double.Parse(AdditionalFee ?? "0")).ToString();
            }
            catch
            {

            }
        }

        //private string _numericValue;
        //public string NumericValue{ get => _numericValue; set => Set(ref _numericValue, value); }

        public RelayCommand<SearchBar> SearchCommand => new(SearchProduct);
        private void SearchProduct(SearchBar searchBar)
        {
            DataList = _dataService.SearchProductList(searchBar.Text);
        }

        public RelayCommand<NumericUpDown> TextInput => new(TextInputCmd);
        private void TextInputCmd(NumericUpDown upDown)
        {
            DeliveryCartModel context = (DeliveryCartModel)upDown.DataContext;
            if(context is DeliveryCartModel)
            {
                if(context.AllowedDecimal && Regex.IsMatch(upDown.Value.ToString(), "[a-zA-Z\\s\\p{P}]+"))
                {
                    //Growl.Info("RAWAWR");
                    upDown.Value = 1;
                    context.Quantity = "1";
                }
            }
            ValueChanged();
        }


        public string ForeColor { get => GlobalData.Config.Theme == HandyControl.Themes.ApplicationTheme.Light ? "Black" : "White"; }
        public string FullName { get => GlobalData.Config.FullName;  }

        public RelayCommand<DataGrid> SelectedCommand => new(AddToCart);
        private void AddToCart(DataGrid obj)
        {
            if(obj.SelectedItems.Count > 0)
            {
                ProductModel pModel = (ProductModel)obj.SelectedItem;
                bool pexists = false;
                foreach(DeliveryCartModel dModel in deliveryCartModels)
                {
                    DeliveryCartModel pCartModel = dModel;
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
                        Quantity = "1",
                        AllowedDecimal = pModel.AllowDecimal
                    };
                    deliveryCartModels.Add(deliveryCartModel);
                }
                ValueChanged();
            }
            obj.SelectedItems.Clear();
            obj.SelectedCells.Clear();
        }

        public RelayCommand<DeliveryCartModel> RemoveCommand => new(RemoveProduct);
        private void RemoveProduct(DeliveryCartModel cartItem)
        {
            deliveryCartModels.Remove(cartItem);
            ValueChanged();
        }

        public RelayCommand AddInventory => new(InsertInventory);
        private void InsertInventory()
        {
            if(string.IsNullOrEmpty(Model.ReferenceNumber))
            {
                Growl.Info("Please provide a reference number.");
                return;
            }
            SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
            SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            SqlCommand sqlCommand;
            try
            {
                if(string.IsNullOrEmpty(Model.SupplierID))
                {
                    Growl.Info("Please select a supplier.");
                    return;
                }
                sqlCommand = new("SELECT COUNT(*) FROM tbldeliveryheaders WHERE invoice_number LIKE @in", sqlConnection, sqlTransaction);
                sqlCommand.Parameters.AddWithValue("@in", Model.ReferenceNumber);
                if((int)sqlCommand.ExecuteScalar() > 0)
                {
                    Growl.Info("Reference number exists!.");
                    return;
                }
                double totalItems = 0d;
                double totalCost = 0d;

                foreach(DeliveryCartModel dModel in deliveryCartModels)
                {
                    totalCost += double.Parse(dModel.Total);
                    totalItems += double.Parse(dModel.Quantity);
                }
                Model.DueTotal = (totalCost + double.Parse(Model.AdditionalFee ?? "0")).ToString();


                sqlCommand = new(@"INSERT INTO tbldeliveryheaders (
                                       supplier_id, user_id, invoice_number, additional_fee,
                                       due_total, note
                                    ) VALUES (
                                        @supid, @uid, @in, @af, @dt, @n
                                    )", sqlConnection, sqlTransaction);
                sqlCommand.Parameters.AddWithValue("@supid", Model.SupplierID);
                sqlCommand.Parameters.AddWithValue("@uid", GlobalData.Config.UserID);
                sqlCommand.Parameters.AddWithValue("@in", Model.ReferenceNumber);
                sqlCommand.Parameters.AddWithValue("@af", Model.AdditionalFee ?? "0");
                sqlCommand.Parameters.AddWithValue("@dt", Model.DueTotal);
                sqlCommand.Parameters.AddWithValue("@n", string.IsNullOrEmpty(Model.Note) ? DBNull.Value : Model.Note);
                if(sqlCommand.ExecuteNonQuery() > 0)
                {
                    foreach(DeliveryCartModel dModel in deliveryCartModels)
                    {
                        sqlCommand = new(@"INSERT INTO tbldeliveryproducts VALUES (
                                            (SELECT TOP 1 id FROM tbldeliveryheaders ORDER BY id DESC), 
                                            @pid,
                                            @quantity,
                                            @total
                                         )", sqlConnection, sqlTransaction);
                        sqlCommand.Parameters.AddWithValue("@pid", dModel.ProductID);
                        sqlCommand.Parameters.AddWithValue("@quantity", dModel.Quantity);
                        sqlCommand.Parameters.AddWithValue("@total", (double.Parse(dModel.Cost) * double.Parse(dModel.Quantity)).ToString());
                        if(sqlCommand.ExecuteNonQuery() == 0)
                        {
                            throw new Exception();
                        }
                    }

                    foreach(DeliveryCartModel dModel in deliveryCartModels)
                    {
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
                    mw?.UpdateAll();
                    WinHelper.AuditActivity("ADDED", "DELIVERY");
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
