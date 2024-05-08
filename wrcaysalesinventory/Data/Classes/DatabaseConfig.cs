using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wrcaysalesinventory.Data.Classes
{
    public class DatabaseConfig
    {
        public static bool Init()
        {
            bool res = false;
            try
            {
                SqlConnection sqlConnection = SqlBaseConnection.GetInstance();
                SqlCommand sqlCommand = new("SELECT COUNT(*) FROM tblstatus;", sqlConnection);
                if ((int)sqlCommand.ExecuteScalar() <= 0)
                {
                    sqlCommand.CommandText = "DBCC CHECKIDENT (tblstatus, RESEED, 0)";
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "INSERT INTO tblstatus VALUES ('Active'), ('Inactive');";
                    if((int)sqlCommand.ExecuteNonQuery() > 0)
                    {
                        res = true;
                    }
                }

                sqlCommand.CommandText = "SELECT COUNT(*) FROM tblroles;";
                if((int)sqlCommand.ExecuteScalar() <= 0)
                {
                    sqlCommand.CommandText = "DBCC CHECKIDENT (tblroles, RESEED, 0)";
                    sqlCommand.ExecuteNonQuery();
                    sqlCommand.CommandText = "INSERT INTO tblroles VALUES ('Super Admin'), ('Admin'), ('Staff');";
                    if ((int)sqlCommand.ExecuteNonQuery() > 0)
                    {
                        res = true;
                    }
                }

            } catch
            {
                return false;
            }
            return res;
        }

        public static bool CheckDatabase(string connection)
        {
            SqlConnection sqlConn = new SqlConnection(connection);
            try
            {
                sqlConn.Open();
                SqlCommand sqlCmd = new("SELECT COUNT(*) FROM sys.tables", sqlConn);
                if((int)sqlCmd.ExecuteScalar() == 13)
                {
                    return true;
                }
                return false;
            } catch
            {
                return false;
            }
        }
    }
}
