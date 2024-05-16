using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.DataSet;
using wrcaysalesinventory.Forms;

namespace wrcaysalesinventory.Customs.Panels
{
    /// <summary>
    /// Interaction logic for ExpensesPanel.xaml
    /// </summary>
    public partial class ExpensesPanel : Grid
    {
        public ExpensesPanel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = SqlBaseConnection.GetInstance();
            SqlCommand cmd = new(@"SELECT dh.id,
	                                   CONCAT(u.first_name,' ', u.last_name) cashier_name,
                                       s.supplier_name,
	                                   invoice_number,
	                                   additional_fee,
	                                   due_total,
	                                   FORMAT(delivery_date, 'dd/MM/yyyy') delivery_date
		
                                FROM
	                                tbldeliveryheaders dh
                                JOIN
	                                tblsuppliers s
	                                ON dh.supplier_id = s.id
                                JOIN
	                                tblusers u
	                                ON dh.user_id = u.id", conn);
            SqlDataAdapter adapter = new(cmd);
            ReportPreview rp = new();
            ReportsDataSet.ExpensesReportDataTable transactionTable = new();
            adapter.Fill(transactionTable);
            ReportDataSource reportDataSource = new()
            {
                Name = "ExpRep",
                Value = transactionTable
            };



            rp.ReportViewer.LocalReport.DataSources.Clear();
            rp.ReportViewer.LocalReport.DataSources.Add(reportDataSource);
            rp.ReportViewer.LocalReport.ReportPath = @"..\..\..\wrcaysalesinventory\Forms\ExpensesReport.rdlc";
            rp.ReportViewer.LocalReport.Refresh();
            rp.ReportViewer.RefreshReport();
            rp.ShowDialog();
        }
    }
}
