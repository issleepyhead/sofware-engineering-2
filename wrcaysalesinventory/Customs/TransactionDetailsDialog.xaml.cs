using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Printing;
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

namespace wrcaysalesinventory.Customs
{
    /// <summary>
    /// Interaction logic for TransactionDetailsDialog.xaml
    /// </summary>
    public partial class TransactionDetailsDialog : UserControl
    {
        private string _fheader;
        private ObservableCollection<TransactionSModel> tlist { get; set; } = new();
        public TransactionDetailsDialog(string fheader)
        {
            InitializeComponent();













            _fheader = fheader;
            DataContext = this;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if(!string.IsNullOrEmpty(_fheader))
                {
                    SqlConnection connection = SqlBaseConnection.GetInstance();
                    SqlCommand sqlCommand = new("SELECT tp.id, product_name , quantity, quantity * product_price total FROM tbltransactionproducts tp JOIN tblproducts p ON tp.product_id = p.id WHERE header_id = @hid", connection);
                    sqlCommand.Parameters.AddWithValue("@hid", _fheader);
                    DataTable dt = new();
                    SqlDataAdapter adapter = new(sqlCommand);
                    adapter.Fill(dt);

                    foreach(DataRow r in dt.Rows)
                    {
                        TransactionSModel tm = new()
                        {
                            id = r["id"].ToString(),
                            pname = r["product_name"].ToString(),
                            q = r["quantity"].ToString(),
                            t = r["total"].ToString()
                        };
                        tlist.Add(tm);
                    }
                    xdw.ItemsSource = tlist;
                }
            } catch
            {
                
            }
        }
    }


    public class TransactionSModel
    {
        public string id { get; set; }
        public string pname { get; set; }
        public string q { get; set; }
        public string t { get; set; }
    }
}
