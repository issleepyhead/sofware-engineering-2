﻿using HandyControl.Controls;
using HandyControl.Tools.Command;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
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

        private ObservableCollection<POSCartModel> _data = [];
        public ObservableCollection<POSCartModel> CartList { get => _data; set => Set(ref _data, value); }
        private ObservableCollection<CustomerModel> _customerList;
        public ObservableCollection<CustomerModel> CustomerList { get => _customerList; set => Set(ref _customerList, value); }
        private TransactionHeaderModel _header = new();
        public TransactionHeaderModel Header { get => _header; set => Set(ref _header, value); }

        private string _subtotal;
        public string SubTotal { get => _subtotal; set => Set(ref _subtotal, value); }
        private string _totalPay;
        public string TotalAmount { get => _totalPay; set { Set(ref _totalPay, value); Header.TotalAmount = _totalPay; } }
        public double TotalItems { get
            {
                double _totalItems = 0;
                foreach (POSCartModel model in CartList)
                {
                    _totalItems += double.Parse(model.Quantity);
                }
                return _totalItems;
            } }

        //public string Discount { get {
        //        if (Regex.IsMatch(Header.Discount, "^(\\d+)?\\.?(\\d+)$"))
        //        {
        //            return _discount;
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    } set {  Set(ref _discount, value); ValueChanged(); } }
        //private string _additional = "0";
        //public string AdditionalFee { get
        //    {
        //        if (Regex.IsMatch(_additional, "^(\\d+)?\\.?(\\d+)$"))
        //        {
        //            return _additional;
        //        } else
        //        {
        //            return null;
        //        }
        //    } set
        //    {
        //        Set(ref _additional, value);
        //        ValueChanged();
        //    }
        //}

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

        public RelayCommand<POSCartModel> PreviewReceiptCommand => new(ViewReceipt);
        private void ViewReceipt(POSCartModel cartItem)
        {
            //Fix the size of the document
            PrintDialog pd = new();
            ReceiptDocument rd = new();
            FlowDocument fd = rd.FD;
            
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
                SqlCommand cmd;
                cmd = new("INSERT INTO tbltransactionheaders (user_id, customer_id, reference_number, note, total_amount, additional_fee, discount, vat) VALUES(@user_id, @cid, @invoice_number, @note, @total_amount, @af, @d, @v)", conn, sqlTransaction);
                cmd.Parameters.AddWithValue("@user_id", 1);
                cmd.Parameters.AddWithValue("@invoice_number", "12213131");
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
                    FlowDocument fd = rd.FD;
                    GenReceipt(pd, fd, rd);
                    mw?.UpdateAll();
                    DiscardCommand(null);
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
            TotalAmount = (total + double.Parse(Header.AdditionalFee ?? "0") - (total * (double.Parse(Header.Discount ?? "0") / 100)) + double.Parse(Header.VAT)).ToString();
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
                    POSCartModel deliveryCartModel = new POSCartModel
                    {
                        ID = pModel.ProductID,
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
                TotalAmount = (total + double.Parse(Header.AdditionalFee ?? "0") - (total * (double.Parse(Header.Discount ?? "0") / 100)) + double.Parse(Header.VAT)).ToString();
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
        }
    }
}
