using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;
using System.Windows.Forms;
using wrcaysalesinventory.Data.Classes;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Properties;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for DeliveryCartDialog.xaml
    /// </summary>
    public partial class DeliveryCartDialog : Border
    {
        private ObservableCollection<DeliveryCartModel> _cartModel;
        public DeliveryCartDialog()
        {
            InitializeComponent();
        }

        private void Border_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            SqlConnection conn = SqlBaseConnection.GetInstance();
            SqlCommand cmd = new("SELECT id, supplier_name FROM tblsuppliers", conn);
            DataTable dataTable = new();
            SqlDataAdapter adapter = new(cmd);
            adapter.Fill(dataTable);
            SupplierNameComboBox.ItemsSource = dataTable.DefaultView;
            SupplierNameComboBox.DisplayMemberPath = "supplier_name";
            SupplierNameComboBox.SelectedValuePath = "id";
            SupplierNameComboBox.SelectedIndex = 0;
        }

        private void SearchTextBox_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            ObservableCollection<ProductModel> pList = new();
            try
            {
                SqlConnection sql= new SqlConnection(Settings.Default.connStr);
                SqlCommand sqlCommand = new(@"SELECT p.id,
                                   c.id category_id,
                                   p.product_status,
                                   c.category_name,
                                   product_name,
                                   product_description,
                                   product_price,
                                   product_cost,
                                   product_unit,
                                   selling_unit,
                                   s.status_name,
                                   FORMAT(p.date_added, 'dd/MM/yyyy') date_added,
                                   FORMAT(p.date_updated, 'dd/MM/yyyy') date_updated
                            FROM tblproducts p
                            JOIN tblstatus s ON p.product_status = s.id
                            JOIN tblcategories c ON p.category_id = c.id;", sql);
                SqlDataAdapter sqlDataAdapter = new(sqlCommand);
                DataTable dt = new();
                sqlDataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    ProductModel pModel = new()
                    {
                        ID = row["id"].ToString(),
                        CategoryID = row["category_id"].ToString(),
                        CategoryName = row["category_name"].ToString(),
                        ProductName = row["product_name"].ToString(),
                        ProductDescription = row["product_description"].ToString(),
                        ProductPrice = row["product_price"].ToString(),
                        ProductCost = row["product_cost"].ToString(),
                        ProductUnit = row["product_unit"].ToString(),
                        DateAdded = row["date_added"].ToString(),
                        DateUpdated = row["date_updated"].ToString(),
                        StatusID = row["product_status"].ToString(),
                        StatusName = row["status_name"].ToString()

                    };
                    pList.Add(pModel);
                }
            }
            catch
            {
                Growl.Warning("An error occured while fetching products");
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductDataGridView.SelectedItems.Count > 0)
            {
                ProductModel productModel = (ProductModel)ProductDataGridView.SelectedItem;
                bool pexists = false;
                int index = 0;
                if(_cartModel.Count < 0)
                {
                    DeliveryCartModel model = new DeliveryCartModel()
                    {
                        ID = productModel.ID,
                        Cost = productModel.ProductCost,
                        Quantity = "1",
                        ProductName = productModel.ProductName
                    };
                    model.Total = (double.Parse(productModel.ProductCost) * double.Parse(model.Quantity)).ToString();
                    _cartModel.Add(model);
                }

                for (int idx = 0; idx < _cartModel.Count; idx++)
                {
                    if (_cartModel[idx].ID == productModel.ID)
                    {
                        index = idx;
                        pexists = true;
                        break;
                    }
                }

                if(!pexists)
                {
                    _cartModel[index].Quantity = (double.Parse(_cartModel[index].Quantity) + 1).ToString();
                }
            }
        }
    }
}
