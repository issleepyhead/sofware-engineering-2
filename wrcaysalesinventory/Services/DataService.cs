using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _sqlCmd = new("", _sqlConn);
            _sqlAdapter = new(_sqlCmd);
            _dataTable = new();
            _sqlAdapter.Fill(_dataTable);
            
            foreach(DataRow row in _dataTable.Rows)
            {
                ProductModel pModel = new()
                {
                    ID = row["id"].ToString(),
                    CategoryID = row["category_id"].ToString(),
                    ProductName = row["product_name"].ToString(),
                    
                    
                };
            }
            return pList;
        }

        internal ObservableCollection<CategoryModel> GetGategoryList()
        {
            ObservableCollection<CategoryModel> cList = new();
            _sqlConn = new SqlConnection(Settings.Default.connStr);
            //TO-DO Create a query.
            _sqlCmd = new("", _sqlConn);
            _sqlAdapter = new(_sqlCmd);
            _dataTable = new();
            _sqlAdapter.Fill(_dataTable);

            foreach (DataRow row in _dataTable.Rows)
            {
                CategoryModel pModel = new()
                {


                };
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
