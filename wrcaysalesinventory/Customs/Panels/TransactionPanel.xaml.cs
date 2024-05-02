using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.ViewModels;

namespace wrcaysalesinventory.Customs.Panels
{
    /// <summary>
    /// Interaction logic for TransactionPanel.xaml
    /// </summary>
    public partial class TransactionPanel : Grid
    {
        public TransactionPanel()
        {
            InitializeComponent();
        }

        private SqlConnection _sqlConn;
        private SqlCommand _sqlCmd;
        private SqlDataAdapter _sqlAdapter;
        private DataTable _dataTable;
        private void ProductSearch_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            ObservableCollection<TransactionModel> sList = new();
            try
            {
                _sqlConn = SqlBaseConnection.GetInstance();
                _sqlCmd = new(@"SELECT t.id,
	                                   customer_id,
	                                   user_id,
	                                   reference_number,
	                                   total_amount,
	                                   additional_fee,
	                                   discount,
	                                   vat,
                                       FORMAT(t.date_added, 'dd/MM/yyyy') date_added,
                                       CONCAT(u.first_name, ' ', u.last_name) FullName
                                FROM 
	                                tbltransactionheaders t
                                LEFT JOIN
	                                tblusers u ON t.user_id = u.id
                                LEFT JOIN
	                                tblcustomers c ON t.customer_id = c.id
                                WHERE reference_number LIKE @ref", _sqlConn);
                string x = ((SearchBar)sender).Text;
                _sqlCmd.Parameters.AddWithValue("@ref", string.IsNullOrEmpty(x) ? "%" : x);
                _sqlAdapter = new(_sqlCmd);
                
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    TransactionModel sModel = new()
                    {
                        ID = row["id"].ToString(),
                        CustomerID = row["customer_id"].ToString(),
                        UserID = row["user_id"].ToString(),
                        ReferenceNumber = row["reference_number"].ToString(),
                        TotalCost = row["total_amount"].ToString(),
                        Discount = row["discount"].ToString(),
                        VAT = row["vat"].ToString(),
                        DateAdded = row["date_added"].ToString(),
                        CashierName = row["FullName"].ToString()

                    };
                    sList.Add(sModel);
                }
                ((TransactionPanelViewModel)DataContext).DataList = sList;
            }
            catch
            {
                Growl.Warning("An error occured while fetching transactions.");
            }
        }
    }
}
