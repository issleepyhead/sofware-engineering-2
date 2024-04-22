using HandyControl.Controls;
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
            try
            {
                _sqlConn = new SqlConnection(Settings.Default.connStr);
                _sqlCmd = new(@"SELECT p.id,
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
                            JOIN tblcategories c ON p.category_id = c.id;", _sqlConn);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
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
            } catch
            {
                Growl.Warning("An error occured while fetching products");
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
                Growl.Warning("An error occured while fetching category list");
            }
            return cList;
        }

        internal ObservableCollection<CategoryModel> GetGategoryPanelList()
        {
            ObservableCollection<CategoryModel> cList = new();
            try
            {
                _sqlConn = SqlBaseConnection.GetInstance();
                _sqlCmd = new("SELECT id,  category_name, category_description, FORMAT(date_added, 'dd/MM/yyyy') date_added, FORMAT(date_updated, 'dd/MM/yyyy') date_updated FROM tblcategories WHERE parent_id IS NULL", _sqlConn);
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
                Growl.Warning("An error occured while fetching category list");
            }
            return cList;
        }

        internal ObservableCollection<SupplierModel> GetSupplierList()
        {
            ObservableCollection<SupplierModel> sList = new();
            try
            {
                _sqlConn = new SqlConnection(Settings.Default.connStr);
                _sqlCmd = new(@"SELECT s.id,
                                   supplier_name,
                                   first_name,
                                   last_name,
                                   city,
                                   country,
                                   address,
                                   phone_number,
                                   FORMAT(s.date_added, 'dd/MM/yyyy') date_added,
                                   FORMAT(s.date_updated, 'dd/MM/yyyy') date_updated,
                                   status_name
                            FROM tblsuppliers s
                            JOIN tblstatus st ON s.status_id = st.id", _sqlConn);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    SupplierModel sModel = new()
                    {
                        ID = row["id"].ToString(),
                        SupplierName = row["supplier_name"].ToString(),
                        FirstName = row["first_name"].ToString(),
                        LastName = row["last_name"].ToString(),
                        City = row["city"].ToString(),
                        Country = row["country"].ToString(),
                        Address = row["address"].ToString(),
                        PhoneNumber = row["phone_number"].ToString(),
                        DateAdded = row["date_added"].ToString(),
                        DateUpdated = row["date_updated"].ToString(),
                        Status = row["status_name"].ToString()
                    };
                    sList.Add(sModel);
                }
            } catch
            {
                Growl.Warning("An error occured while fetching suppliers.");
            }
            return sList;
        }

        internal ObservableCollection<StatusModel> GetStatusList()
        {
            ObservableCollection<StatusModel> sList = new();
            try
            {
                _sqlConn = new SqlConnection(Settings.Default.connStr);
                _sqlCmd = new(@"SELECT id, status_name FROM tblstatus", _sqlConn);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    StatusModel sModel = new()
                    {
                        ID = row["id"].ToString(),
                        StatusName = row["status_name"].ToString()
                    };
                    sList.Add(sModel);
                }
            }
            catch
            {
                Growl.Warning("An error occured while fetching suppliers.");
            }
            return sList;
        }
    }
}
