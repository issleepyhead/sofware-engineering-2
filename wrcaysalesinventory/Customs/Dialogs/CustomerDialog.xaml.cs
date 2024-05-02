using System.Data.SqlClient;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for CustomerDialog.xaml
    /// </summary>
    public partial class CustomerDialog : Border
    {
        public CustomerDialog(POSPanelViewModel vm = null)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CategoryNameError.Text = string.Empty;
            DescriptionError.Text = string.Empty;
            EMR.Text = string.Empty;

            bool has_e = false;
            if (string.IsNullOrEmpty(FN.Text))
            {
                CategoryNameError.Text = "Full Name can't be empty.";
                has_e = true;
            }

            if (string.IsNullOrEmpty(PN.Text))
            {
                DescriptionError.Text = "Please provide a phone number";
                has_e = true;
            }

            if(string.IsNullOrEmpty(EM.Text))
            {
                EMR.Text = "Please provide an email.";
                has_e = true;
            }

            if (has_e)
            {
                return;
            }
            else
            {
                try
                {
                    SqlConnection con = SqlBaseConnection.GetInstance();

                    SqlCommand cmd = new("INSERT INTO tblcustomers (full_name, phone, email) VALUES (@f, @p, @e);", con);
                    cmd.Parameters.AddWithValue("@f", FN.Text);
                    cmd.Parameters.AddWithValue("@p", PN.Text);
                    cmd.Parameters.AddWithValue("@e", EM.Text);
                    cmd.ExecuteNonQuery();
                }
                catch
                {

                }
            }
        }
    }
}
