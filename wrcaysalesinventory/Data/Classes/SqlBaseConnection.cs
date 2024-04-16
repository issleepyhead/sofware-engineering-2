using System.Data.SqlClient;

namespace wrcaysalesinventory.Data.Classes
{
    public class SqlBaseConnection
    {
        public static SqlConnection GetInstance()
        {
            SqlConnection sqlConnection = new("Data Source=I-AM-ROOT;Initial Catalog=wrcaydb;Persist Security Info=True;User ID=clancy;Password=0x0305");
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();
            return sqlConnection;
        }
    }
}
