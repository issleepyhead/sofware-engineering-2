using System.Windows.Controls;
using wrcaysalesinventory.Data.Models;

namespace wrcaysalesinventory.Customs.Dialogs
{
    public partial class ProductCartItem : Border
    {
        public ProductCartItem(DeliveryCartModel cartModel = null)
        { 
            InitializeComponent();
            if(cartModel != null)
                DataContext = cartModel;
        }
    }
}
