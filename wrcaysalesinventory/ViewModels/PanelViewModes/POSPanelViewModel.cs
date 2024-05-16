using HandyControl.Controls;
using HandyControl.Tools.Command;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Forms;
using wrcaysalesinventory.Services;
using Application = System.Windows.Application;
using DataGrid = System.Windows.Controls.DataGrid;
using PrintDialog = System.Windows.Controls.PrintDialog;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class POSPanelViewModel : BaseViewModel<StocksModel>
    {
        private DataService _dataService;
        private readonly MainWindow mw;
        public POSPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            DataList = _dataService.GetStocksList();
            CustomerList = _dataService.GetCustomerList();
            mw = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        }

        private ObservableCollection<POSCartModel> _data = new();
        public ObservableCollection<POSCartModel> CartList { get => _data; set => Set(ref _data, value); }
        private ObservableCollection<CustomerModel> _customerList;
        public ObservableCollection<CustomerModel> CustomerList { get => _customerList; set => Set(ref _customerList, value); }
        private TransactionHeaderModel _header = new();
        public TransactionHeaderModel Header { get => _header; set => Set(ref _header, value); }

        private string _addfeeerr;
        public string AdditionalFeeError { get => _addfeeerr; set => Set(ref _addfeeerr, value); }

        private string _cashAmount;
        public string CashAmount { get => _cashAmount; set { Set(ref _cashAmount, value);  ValueChanged(); } }

        private string _changeAmount;
        public string ChangeAmount { get => _changeAmount; set => Set(ref _changeAmount, value); }

        private string _subtotal = "0";
        public string SubTotal { get => _subtotal; set => Set(ref _subtotal, value); }
        private string _totalPay = "0";
        public string TotalAmount { get => _totalPay; set { Set(ref _totalPay, value); Header.TotalAmount = _totalPay; } }
        public double TotalItems
        {
            get
            {
                double _totalItems = 0;
                foreach (POSCartModel model in CartList)
                {
                    _totalItems += double.Parse(model.Quantity);
                }
                return _totalItems;
            }
        }

        private string _discountError;
        public string DiscountError { get => _discountError; set => Set(ref _discountError, value); }
        private string _discount = "0";
        public string Discount { get => _discount; set { Set(ref _discount, value); } }

        public RelayCommand<object> SaveAddFees => new(SaveAddCmd);
        private void SaveAddCmd(object obj)
        {
            if(!Regex.IsMatch(Header.AdditionalFee, @"^(\d+)?\.?(\d+)$"))
            {
                AdditionalFeeError = "Please provide a valid value";
                return;
            }
            ValueChanged();
            WinHelper.CloseDialog((Button)obj);
        }

        public RelayCommand<SearchBar> SearchCommand => new(SearchProduct);
        private void SearchProduct(SearchBar searchBar)
        {
            DataList = _dataService.SearchStocksList(searchBar.Text);
        }

        public RelayCommand<POSCartModel> RemoveCommand => new(RemoveProduct);
        private void RemoveProduct(POSCartModel cartItem)
        {
            CartList.Remove(cartItem);
            ValueChanged();
        }

        public RelayCommand<NumericUpDown> TextInput => new(TextInputCmd);
        private void TextInputCmd(NumericUpDown upDown)
        {
            POSCartModel context = (POSCartModel)upDown.DataContext;
            try
            {
                if (double.Parse(context.Quantity) > double.Parse(context.Stocks))
                {
                    Growl.Info("Insufficient stocks.");
                    upDown.Value = double.Parse(context.Stocks);
                    context.Quantity = context.Stocks;
                    return;
                }
            } catch
            {
                Debug.WriteLine("Invalid Format");
            }


            if (context is POSCartModel)
            {
                if (context.AllowedDecimal && Regex.IsMatch(upDown.Value.ToString(), "[a-zA-Z\\s\\p{P}]+"))
                {
                    upDown.Value = 1;
                    context.Quantity = "1";
                }
            }
            ValueChanged();
        }

        public string ForeColor { get => GlobalData.Config.Theme == HandyControl.Themes.ApplicationTheme.Light ? "Black" : "White"; }

        public RelayCommand<POSCartModel> PreviewReceiptCommand => new(ViewReceipt);
        private void ViewReceipt(POSCartModel cartItem)
        {
            SqlConnection conn = SqlBaseConnection.GetInstance();
            SqlCommand cmd = new("SELECT COUNT(*) FROM tbltransactionheaders", conn);
            string refer = ((int)cmd.ExecuteScalar() + 1).ToString();
            Header.ReferenceNumber = refer.ToString().PadLeft(8 - refer.Length, '0') + refer;
            //Fix the size of the document
            PrintDialog pd = new();
            ReceiptDocument rd = new();
            FlowDocument fd = rd.FD;
            ((ReceiptViewModel)rd.DataContext).Header = Header;
            ((ReceiptViewModel)rd.DataContext).SubTotal = SubTotal;
            ((ReceiptViewModel)rd.DataContext).AmountReceived = "0";
            GenReceipt(pd,fd, rd);

            if (File.Exists("printPreview.xps")) File.Delete("printPreview.xps");
            var xpsDocument = new XpsDocument("printPreview.xps", FileAccess.ReadWrite);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
            writer.Write(((IDocumentPaginatorSource)fd).DocumentPaginator);
            FixedDocumentSequence fixedDocumentSequence = xpsDocument.GetFixedDocumentSequence();
            xpsDocument.Close();
            var windows = new ReceiptReport(fixedDocumentSequence);
            windows.ShowDialog();
        }

        private void GenReceipt(PrintDialog pd, FlowDocument fds, ReceiptDocument rd)
        {
            fds.ColumnGap = 0;
            fds.ColumnWidth = pd.PrintableAreaWidth;

            rd.ReceiptNumber.Text = Header.ReferenceNumber;


            foreach (POSCartModel cart in CartList)
            {
                TableCell tblpname = new(new Paragraph(new Run(cart.ProductName)));
                //tblpname.TextAlignment = TextAlignment.Center;
                TableCell tblprice = new(new Paragraph(new Run(cart.Cost)));
                //tblprice.TextAlignment = TextAlignment.Center;
                TableCell tblqty = new(new Paragraph(new Run(cart.Quantity)));
                //tblqty.TextAlignment = TextAlignment.Center;
                TableCell tbltotal = new(new Paragraph(new Run(cart.Total)));
                tbltotal.TextAlignment = TextAlignment.Right;
                TableRow row = new();

                row.Cells.Add(tblpname);
                row.Cells.Add(tblprice);
                row.Cells.Add(tblqty);
                row.Cells.Add(tbltotal);

                rd.TableRowsGroupName.Rows.Add(row);
            }
        }

        public RelayCommand<POSCartModel> PayCommand => new(PayTransact);
        private void PayTransact(POSCartModel cartItem)
        {
            SqlConnection conn = SqlBaseConnection.GetInstance();
            SqlTransaction sqlTransaction = conn.BeginTransaction();
            try
            {
                if(string.IsNullOrEmpty(CashAmount))
                {
                    Growl.Info("Please provide a cash amount.");
                    return;
                }

                if (!Regex.IsMatch(CashAmount, "^(\\d+)?\\.?(\\d+)$"))
                {
                    Growl.Info("Please provide a valid cash amount");
                    return;
                }

                if (double.Parse(CashAmount) - double.Parse(TotalAmount) < 0)
                {
                    Growl.Info("Insufficient cash amount.");
                    return;
                }

                SqlCommand cmd;

                cmd = new("SELECT COUNT(*) FROM tbltransactionheaders", conn, sqlTransaction);
                string refer = ((int)cmd.ExecuteScalar() + 1).ToString();
                Header.ReferenceNumber = refer.ToString().PadLeft(8 - refer.Length, '0') + refer;

                Header.Discount = _discount;
                cmd = new("INSERT INTO tbltransactionheaders (user_id, customer_id, reference_number, note, total_amount, additional_fee, discount, vat) VALUES(@user_id, @cid, @invoice_number, @note, @total_amount, @af, @d, @v)", conn, sqlTransaction);
                cmd.Parameters.AddWithValue("@user_id", GlobalData.Config.UserID);
                cmd.Parameters.AddWithValue("@invoice_number", Header.ReferenceNumber);
                cmd.Parameters.AddWithValue("@total_amount", string.IsNullOrEmpty(Header.TotalAmount) ? DBNull.Value : Header.TotalAmount);
                cmd.Parameters.AddWithValue("@af", string.IsNullOrEmpty(Header.AdditionalFee) ? DBNull.Value : Header.AdditionalFee);
                cmd.Parameters.AddWithValue("@d", string.IsNullOrEmpty(Header.Discount) ? DBNull.Value : Header.Discount);
                cmd.Parameters.AddWithValue("@v", string.IsNullOrEmpty(Header.VAT) ? DBNull.Value : Header.VAT);
                cmd.Parameters.AddWithValue("@cid", string.IsNullOrEmpty(Header.CustomerID) ? DBNull.Value : Header.CustomerID);
                cmd.Parameters.AddWithValue("@note", string.IsNullOrEmpty(Header.Note) ? DBNull.Value : Header.Note);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    foreach (POSCartModel cart in CartList)
                    {

                        cmd = new("UPDATE tblinventory SET stocks = stocks - @quantity, sold = sold + @quantity WHERE product_id = @product_id", conn, sqlTransaction);
                        cmd.Parameters.AddWithValue("@quantity", cart.Quantity);
                        cmd.Parameters.AddWithValue("@product_id", cart.ID);
                        if ((int)cmd.ExecuteNonQuery() == 0)
                        {
                            throw new Exception();
                        }
                    }

                    foreach (POSCartModel cart in CartList)
                    {
                        cmd = new("INSERT INTO tbltransactionproducts (header_id, product_id, quantity) VALUES ((SELECT TOP 1 id FROM tbltransactionheaders ORDER BY id DESC), @product_id, @quantity) ", conn, sqlTransaction);
                        cmd.Parameters.AddWithValue("@quantity", cart.Quantity);
                        cmd.Parameters.AddWithValue("@product_id", cart.ID);
                        if ((int)cmd.ExecuteNonQuery() == 0)
                        {
                            throw new Exception();
                        }
                    }
                    sqlTransaction.Commit();
                    CartList.Clear();
                    Growl.Success("Transaction added successfully");
                    PrintDialog pd = new();
                    ReceiptDocument rd = new();
                    ((ReceiptViewModel)rd.DataContext).Header = Header;
                    ((ReceiptViewModel)rd.DataContext).SubTotal = SubTotal;
                    ((ReceiptViewModel)rd.DataContext).AmountReceived = "0";
                    FlowDocument fd = rd.FD;
                    GenReceipt(pd, fd, rd);


                    if (File.Exists("printPreview.xps")) File.Delete("printPreview.xps");
                    var xpsDocument = new XpsDocument("printPreview.xps", FileAccess.ReadWrite);
                    XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
                    writer.Write(((IDocumentPaginatorSource)fd).DocumentPaginator);
                    FixedDocumentSequence fixedDocumentSequence = xpsDocument.GetFixedDocumentSequence();
                    xpsDocument.Close();
                    

                    mw?.UpdateAll();
                    DataList = _dataService.GetStocksList();
                    DiscardCommand(null);
                    WinHelper.AuditActivity("ADDED", "TRANSACTION");
                }

            }
            catch
            {
                sqlTransaction.Rollback();
            }
            finally
            {
                conn.Dispose();
            }
        }


        public void ValueChanged()
        {
            double total = 0;
            foreach (POSCartModel model in CartList)
            {
                total += double.Parse(model.Total);
            }
            SubTotal = total.ToString();
            total += (total * (double.Parse(Header.VAT) / 100)) - (total * (double.Parse(Discount ?? "0") / 100));
            TotalAmount = (total + double.Parse(Header.AdditionalFee ?? "0")).ToString();
            if (string.IsNullOrEmpty(CashAmount))
            {
                return;
            }
            if (!string.IsNullOrEmpty(CashAmount) && !Regex.IsMatch(CashAmount, "^(\\d+)?\\.?(\\d+)$"))
            {
                Growl.Info("Please provide a valid cash amount");
                return;
            }
            ChangeAmount = (double.Parse(CashAmount ?? "0") - double.Parse(TotalAmount)).ToString();
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
                    if (pCartModel.ID == pModel.ProductID)
                    {
                        pexists = true;
                        break;
                    }
                }

                if (!pexists)
                {
                    POSCartModel deliveryCartModel = new()
                    {
                        ID = pModel.ProductID,
                        Cost = pModel.Cost,
                        ProductName = pModel.ProductName,
                        Quantity = "1",
                        AllowedDecimal = pModel.AllowedDecimal,
                        Stocks = pModel.Stocks
                    };
                    CartList.Add(deliveryCartModel);
                }
                ValueChanged();
            }
            dataGrid.SelectedItems.Clear();
            dataGrid.SelectedCells.Clear();
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
            CustomerDialog d = new();
            Dialog.Show(d);
        }

        public RelayCommand<object> OpenNoteCmd => new(OpenNote);

        private void OpenNote(object obj)
        {
            NoteDialog d = new(Header);
            Dialog.Show(d);
        }

        public RelayCommand<object> DiscardCmd => new(DiscardCommand);

        private void DiscardCommand(object obj)
        {
            CartList?.Clear();
            Header = new();
            SubTotal = "0";
            TotalAmount = "0";
            Discount = "0";
        }
    }
}
