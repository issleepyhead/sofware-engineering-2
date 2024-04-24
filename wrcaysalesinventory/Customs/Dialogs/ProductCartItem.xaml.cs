using System.Windows.Controls;
using wrcaysalesinventory.Data.Models;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for ProductCartItem.xaml
    /// </summary>
    public partial class ProductCartItem : Border
    {
        public ProductCartItem(DeliveryCartModel dmodel = null)
        {
            InitializeComponent();
            if(dmodel != null)
                DataContext = dmodel;
        }
    }
}
