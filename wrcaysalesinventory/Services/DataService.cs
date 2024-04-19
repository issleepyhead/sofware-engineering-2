using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Properties;

namespace wrcaysalesinventory.Services
{
    public class DataService
    {
        private SqlConnection _sqlConn;
        private SqlCommand _sqlCmd;
        private SqlDataAdapter _sqlAdapter;
        private DataTable _dataTable;


        internal ObservableCollection<ProductModel> GetProductList()
        {
            ObservableCollection<ProductModel> pList = new();
            _sqlConn = new SqlConnection(Settings.Default.connStr);
            //TO-DO Create a query.
            _sqlCmd = new(@"SELECT p.id,
                                   c.id category_id,
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
                            JOIN tblcategories c ON p.category_id = c.id;", _sqlConn);
            _sqlAdapter = new(_sqlCmd);
            _dataTable = new();
            _sqlAdapter.Fill(_dataTable);
            
            foreach(DataRow row in _dataTable.Rows)
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
                    DateUpdated = row["date_updated"].ToString()
                };
                pList.Add(pModel);
            }
            return pList;
        }

        internal ObservableCollection<CategoryModel> GetGategoryList()
        {
            ObservableCollection<CategoryModel> cList = new();
            try
            {
                _sqlConn = SqlBaseConnection.GetInstance();
                _sqlCmd = new("SELECT * FROM tblcategories", _sqlConn);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    CategoryModel model = new()
                    {
                        ID = row["id"].ToString(),
                        CategoryName = row["category_name"].ToString(),
                        CategoryDescription = row["category_description"].ToString(),

                    };
                    cList.Add(model);
                }
            } catch
            {
                Debug.WriteLine("Error while fetching category list");
            }
            return cList;
        }

        internal ObservableCollection<CategoryModel> GetGategoryPanelList()
        {
            ObservableCollection<CategoryModel> cList = new();
            try
            {
                _sqlConn = SqlBaseConnection.GetInstance();
                _sqlCmd = new("SELECT * FROM tblcategories WHERE parent_id IS NULL", _sqlConn);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    CategoryModel model = new()
                    {
                        ID = row["id"].ToString(),
                        CategoryName = row["category_name"].ToString(),
                        CategoryDescription = row["category_description"].ToString(),
                        DateAdded = row["date_added"].ToString(),
                        DateUpdated = row["date_updated"].ToString()
                    };
                    cList.Add(model);
                }
            }
            catch
            {
                Debug.WriteLine("Error while fetching category list");
            }
            return cList;
        }

        internal ObservableCollection<SupplierModel> GetSupplierList()
        {
            ObservableCollection<SupplierModel> sList = new();
            _sqlConn = new SqlConnection(Settings.Default.connStr);
            //TO-DO Create a query.
            _sqlCmd = new("", _sqlConn);
            _sqlAdapter = new(_sqlCmd);
            _dataTable = new();
            _sqlAdapter.Fill(_dataTable);

            foreach (DataRow row in _dataTable.Rows)
            {
                SupplierModel pModel = new()
                {


                };
            }
            return sList;
        }
    }
}
