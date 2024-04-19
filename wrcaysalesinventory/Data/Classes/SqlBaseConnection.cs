using System.Data.SqlClient;
using wrcaysalesinventory.Properties;

namespace wrcaysalesinventory.Data.Classes
{
    public class SqlBaseConnection
    {
        public static SqlConnection GetInstance()
        {
            SqlConnection sqlConnection = new(Settings.Default.connStr);
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();
            return sqlConnection;
        }
    }
}
