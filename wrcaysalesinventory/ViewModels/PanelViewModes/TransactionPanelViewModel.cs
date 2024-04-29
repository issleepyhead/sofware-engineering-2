using HandyControl.Tools.Command;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.DataSet;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Forms;
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

        public RelayCommand<object> OpenReportCmd => new(ReportCommand);
        private void ReportCommand(object obj)
        {
            SqlConnection conn = SqlBaseConnection.GetInstance();
            SqlCommand cmd = new(@"SELECT t.id,
	                                   user_id,
	                                   reference_number,
	                                   total_amount,
	                                   additional_fee,
	                                   CASE WHEN discount IS NULL THEN '0%' ELSE CONCAT(discount, '%') END AS discount,
	                                   CASE WHEN vat IS NULL THEN '0%' ELSE CONCAT(vat,'%') END AS vat,
                                       FORMAT(t.date_added, 'dd/MM/yyyy') date_added,
                                       CONCAT(u.first_name, ' ', u.last_name) cashier_name
                                FROM 
	                                tbltransactionheaders t
                                LEFT JOIN
	                                tblusers u ON t.user_id = u.id", conn);
            SqlDataAdapter adapter = new(cmd);
            ReportPreview rp = new();
            ReportsDataSet.TransactionReportDataTable transactionTable = new();
            adapter.Fill(transactionTable);
            ReportDataSource reportDataSource = new()
            {
                Name = "TransactionReporting",
                Value = transactionTable
            };



            rp.ReportViewer.LocalReport.DataSources.Clear();
            rp.ReportViewer.LocalReport.DataSources.Add(reportDataSource);
            rp.ReportViewer.LocalReport.ReportPath = @"..\..\..\wrcaysalesinventory\Forms\SalesReport.rdlc";
            rp.ReportViewer.LocalReport.Refresh();
            rp.ReportViewer.RefreshReport();
            rp.ShowDialog();
        }
    }
}
