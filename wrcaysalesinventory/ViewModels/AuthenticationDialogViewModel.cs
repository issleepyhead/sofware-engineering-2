using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wrcaysalesinventory.Data.Classes;

namespace wrcaysalesinventory.ViewModels
{
    public class AuthenticationDialogViewModel : ViewModelBase
    {
        private string _password;

        public string Password { get => _password; set => Set(ref _password, value); }

        public RelayCommand<object> Authenticate => new(AuthenticateCmd);
        public void AuthenticateCmd(object obj)
        {
            
            try
            {
                SqlConnection conn = SqlBaseConnection.GetInstance();
                SqlCommand cmd = new("SELECT password FROM tblusers WHERE id = @id;", conn);
                cmd.Parameters.AddWithValue("@id", GlobalData.Config.UserID);
                SqlDataAdapter adapter = new(cmd);
                DataTable dataTable = new();
                adapter.Fill(dataTable);

                if(dataTable.Rows.Count > 0)
                {
                    string password = dataTable.Rows[0]["password"].ToString();
                    if(BCrypt.Net.BCrypt.Verify(Password, password))
                    {

                    }
                }
            }
            catch
            {

            }
        }
    }
}
