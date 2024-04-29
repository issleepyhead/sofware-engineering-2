using HandyControl.Controls;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
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
            ObservableCollection<ProductModel> pList = [];
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
                        StatusName = row["status_name"].ToString(),
                        NotAllowDecimal = row["selling_unit"].ToString() == "1",

                    };
                    pList.Add(pModel);
                }
            } catch
            {
                Growl.Warning("An error occured while fetching products");
            }
            return pList;
        }
        internal ObservableCollection<ProductModel> SearchProductList(string query)
        {
            ObservableCollection<ProductModel> pList = [];
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
                            JOIN tblcategories c ON p.category_id = c.id WHERE product_name LIKE @pname;", _sqlConn);
                _sqlCmd.Parameters.AddWithValue("@pname", query);
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
            }
            catch
            {
                Growl.Warning("An error occured while fetching products");
            }
            return pList;
        }
        internal ObservableCollection<CategoryModel> GetGategoryList()
        {
            ObservableCollection<CategoryModel> cList = [];
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
            ObservableCollection<CategoryModel> cList = [];
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
        internal ObservableCollection<CategoryModel> SearchGategoryPanelList(string query)
        {
            ObservableCollection<CategoryModel> cList = [];
            try
            {
                _sqlConn = SqlBaseConnection.GetInstance();
                _sqlCmd = new("SELECT id,  category_name, category_description, FORMAT(date_added, 'dd/MM/yyyy') date_added, FORMAT(date_updated, 'dd/MM/yyyy') date_updated FROM tblcategories WHERE parent_id IS NULL AND category_name LIKE @cname", _sqlConn);
                _sqlCmd.Parameters.AddWithValue("@cname", query);
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
            ObservableCollection<SupplierModel> sList = [];
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
            ObservableCollection<StatusModel> sList = [];
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
        internal ObservableCollection<RoleModel> GetRoleList()
        {
            ObservableCollection<RoleModel> sList = [];
            try
            {
                _sqlConn = SqlBaseConnection.GetInstance();
                _sqlCmd = new(@"SELECT id, role_name FROM tblroles", _sqlConn);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    RoleModel sModel = new()
                    {
                        ID = row["id"].ToString(),
                        RoleName = row["role_name"].ToString()
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
        internal ObservableCollection<UserModel> GetUsersList()
        {
            ObservableCollection<UserModel> sList = [];
            try
            {
                _sqlConn = new SqlConnection(Settings.Default.connStr);
                _sqlCmd = new(@"SELECT a.id,
	                                   s.id status_id,
	                                   s.status_name,
	                                   r.id role_id,
	                                   r.role_name,
	                                   first_name,
	                                   last_name,
	                                   address,
	                                   contact,
	                                   username,
	                                   password,
	                                   FORMAT(date_added, 'dd/MM/yyyy') date_added
                                FROM
	                                tblusers a
                                JOIN
	                                tblstatus s ON a.status_id = s.id
                                JOIN
	                                tblroles r ON a.role_id = r.id", _sqlConn);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    UserModel sModel = new()
                    {
                        ID = row["id"].ToString(),
                        StatusName = row["status_name"].ToString(),
                        StatusID = row["status_id"].ToString(),
                        RoleID = row["role_id"].ToString(),
                        RoleName = row["role_name"].ToString(),
                        FirstName = row["first_name"].ToString(),
                        LastName = row["last_name"].ToString(),
                        Address = row["address"].ToString(),
                        Contact = row["contact"].ToString(),
                        Username = row["username"].ToString(),
                        Password = row["password"].ToString(),
                        DateAdded = row["date_added"].ToString()
                    };
                    sList.Add(sModel);
                }
            }
            catch
            {
                Growl.Warning("An error occured while fetching users.");
            }
            return sList;
        }
        internal ObservableCollection<DeliveryModel> GetDeliveryList()
        {
            ObservableCollection<DeliveryModel> sList = [];
            try
            {
                _sqlConn = new SqlConnection(Settings.Default.connStr);
                _sqlCmd = new(@"SELECT dh.id,
	                                   dh.supplier_id,
	                                   dh.user_id,
	                                   u.first_name,
	                                   u.last_name,
                                       s.supplier_name,
	                                   invoice_number,
	                                   additional_fee,
	                                   total_items,
	                                   due_total,
	                                   note,
	                                   FORMAT(delivery_date, 'dd/MM/yyyy') delivery_date
		
                                FROM
	                                tbldeliveryheaders dh
                                JOIN
	                                tblsuppliers s
	                                ON dh.supplier_id = s.id
                                JOIN
	                                tblusers u
	                                ON dh.user_id = u.id", _sqlConn);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    DeliveryModel sModel = new()
                    {
                        ID = row["id"].ToString(),
                        UserID = row["user_id"].ToString(),
                        SupplierID = row["supplier_id"].ToString(),
                        FullName = row["first_name"].ToString() + " " + row["last_name"].ToString(),
                        AdditionalFee = row["additional_fee"].ToString(),
                        DueTotal = row["due_total"].ToString(),
                        TotalItems = row["total_items"].ToString(),
                        Note = row["note"].ToString(),
                        DeliveryDate = row["delivery_date"].ToString(),
                        SupplierName = row["supplier_name"].ToString(),
                        ReferenceNumber = row["invoice_number"].ToString(),
                    };
                    sList.Add(sModel);
                }
            }
            catch
            {
                Growl.Warning("An error occured while fetching deliveries.");
            }
            return sList;
        }
        internal ObservableCollection<StocksModel> GetStocksList()
        {
            ObservableCollection<StocksModel> sList = [];
            try
            {
                _sqlConn = new SqlConnection(Settings.Default.connStr);
                _sqlCmd = new(@"SELECT i.id,
	                                   p.product_name,
	                                   st.status_name,
	                                   stocks,
	                                   sold,
	                                   defective,
                                       p.product_unit,
                                       p.product_cost,
                                       i.product_id
                                FROM
	                                tblinventory i
                                JOIN
	                                tblproducts p ON i.product_id = p.id
                                JOIN
	                                tblstatus st ON p.product_status = st.id", _sqlConn);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    StocksModel sModel = new()
                    {
                        ID = row["id"].ToString(),
                        StatusName = row["status_name"].ToString(),
                        ProductName = row["product_name"].ToString(),
                        Stocks = row["stocks"].ToString(),
                        Sold = row["sold"].ToString(),
                        Defective = row["defective"].ToString(),
                        StoksUnit = row["stocks"].ToString() +" "+ row["product_unit"].ToString(),
                        Cost = row["product_cost"].ToString(),
                        ProductID = row["product_id"].ToString()
                    };
                    sList.Add(sModel);
                }
            }
            catch
            {
                Growl.Warning("An error occured while fetching stocks");
            }
            return sList;
        }
        internal ObservableCollection<StocksModel> SearchStocksList(string query)
        {
            ObservableCollection<StocksModel> sList = [];
            try
            {
                _sqlConn = new SqlConnection(Settings.Default.connStr);
                _sqlCmd = new(@"SELECT i.id,
	                                   p.product_name,
	                                   st.status_name,
                                       p.product_unit,
	                                   stocks,
	                                   sold,
	                                   defective,
                                       p.product_cost
                                FROM
	                                tblinventory i
                                JOIN
	                                tblproducts p ON i.product_id = p.id
                                JOIN
	                                tblstatus st ON p.product_status = st.id
                                WHERE product_name LIKE @pname", _sqlConn);
                _sqlCmd.Parameters.AddWithValue("@pname", string.IsNullOrEmpty(query) ? "%" : query);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    StocksModel sModel = new()
                    {
                        ID = row["id"].ToString(),
                        StatusName = row["status_name"].ToString(),
                        ProductName = row["product_name"].ToString(),
                        Stocks = row["stocks"].ToString(),
                        Sold = row["sold"].ToString(),
                        Defective = row["defective"].ToString(),
                        StoksUnit = row["stocks"].ToString() + " " + row["product_unit"].ToString(),
                        Cost = row["product_cost"].ToString()
                    };
                    sList.Add(sModel);
                }
            }
            catch
            {
                Growl.Warning("An error occured while fetching stocks");
            }
            return sList;
        }
        internal ObservableCollection<SupplierModel> SearchSupplierList(string query)
        {
            ObservableCollection<SupplierModel> sList = [];
            try
            {
                _sqlConn = SqlBaseConnection.GetInstance();
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
                            JOIN tblstatus st ON s.status_id = st.id
                            WHERE supplier_name LIKE @query", _sqlConn);
                _sqlCmd.Parameters.AddWithValue("@query", string.IsNullOrEmpty(query) ? "%" : query);
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
            }
            catch
            {
                Growl.Warning("An error occured while fetching suppliers.");
            }
            return sList;
        }
        internal ObservableCollection<UserModel> SearchUsersList(string query)
        {
            ObservableCollection<UserModel> sList = [];
            try
            {
                _sqlConn = SqlBaseConnection.GetInstance();
                _sqlCmd = new(@"SELECT a.id,
	                                   s.id status_id,
	                                   s.status_name,
	                                   r.id role_id,
	                                   r.role_name,
	                                   first_name,
	                                   last_name,
	                                   address,
	                                   contact,
	                                   username,
	                                   password,
	                                   FORMAT(date_added, 'dd/MM/yyyy') date_added
                                FROM
	                                tblusers a
                                JOIN
	                                tblstatus s ON a.status_id = s.id
                                JOIN
	                                tblroles r ON a.role_id = r.id
                                WHERE username LIKE @query OR first_name LIKE @query OR last_name LIKE @query", _sqlConn);
                _sqlCmd.Parameters.AddWithValue("@query", query);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    UserModel sModel = new()
                    {
                        ID = row["id"].ToString(),
                        StatusName = row["status_name"].ToString(),
                        StatusID = row["status_id"].ToString(),
                        RoleID = row["role_id"].ToString(),
                        RoleName = row["role_name"].ToString(),
                        FirstName = row["first_name"].ToString(),
                        LastName = row["last_name"].ToString(),
                        Address = row["address"].ToString(),
                        Contact = row["contact"].ToString(),
                        Username = row["username"].ToString(),
                        Password = row["password"].ToString(),
                        DateAdded = row["date_added"].ToString()
                    };
                    sList.Add(sModel);
                }
            }
            catch
            {
                Growl.Warning("An error occured while fetching users.");
            }
            return sList;
        }
        internal ObservableCollection<TransactionModel> GetTransactionList()
        {
            ObservableCollection<TransactionModel> sList = [];
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
	                                tblcustomers c ON t.customer_id = c.id", _sqlConn);
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
            }
            catch
            {
                Growl.Warning("An error occured while fetching transactions.");
            }
            return sList;
        }
        internal ObservableCollection<CustomerModel> GetCustomerList()
        {
            ObservableCollection<CustomerModel> sList = [];
            try
            {
                _sqlConn = SqlBaseConnection.GetInstance();
                _sqlCmd = new(@"SELECT id, full_name, phone, email FROM tblcustomers", _sqlConn);
                _sqlAdapter = new(_sqlCmd);
                _dataTable = new();
                _sqlAdapter.Fill(_dataTable);

                foreach (DataRow row in _dataTable.Rows)
                {
                    CustomerModel sModel = new()
                    {
                        ID = row["id"].ToString(),
                        FullName = row["full_name"].ToString(),
                        Phone = row["phone"].ToString(),
                        Email = row["email"].ToString()
                    };
                    sList.Add(sModel);
                }
            }
            catch
            {
                Growl.Warning("An error occured while fetching customers.");
            }
            return sList;
        }

        internal ObservableCollection<ProductModel> GetProductByCategoryList(string cid)
        {
            ObservableCollection<ProductModel> pList = [];
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
                            JOIN tblcategories c ON p.category_id = c.id
                            WHERE c.id = @cid;", _sqlConn);
                _sqlCmd.Parameters.AddWithValue("@cid", cid);
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
            }
            catch
            {
                Growl.Warning("An error occured while fetching products");
            }
            return pList;
        }
    }
}
