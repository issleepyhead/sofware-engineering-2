using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;
using System.Windows.Forms;
using wrcaysalesinventory.Data.Classes;
using HandyControl.Controls;
using System.Collections.ObjectModel;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Properties;
using wrcaysalesinventory.ViewModels.PanelViewModes;
using wrcaysalesinventory.ViewModels;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for DeliveryCartDialog.xaml
    /// </summary>
    public partial class DeliveryCartDialog : Border
    {
        //private ObservableCollection<DeliveryCartModel> _cartModel;
        public DeliveryCartDialog()
        {
            InitializeComponent();
        }

    }
}
