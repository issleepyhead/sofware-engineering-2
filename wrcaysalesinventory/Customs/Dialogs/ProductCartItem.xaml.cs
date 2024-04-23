using System.Windows.Controls;
using wrcaysalesinventory.Data.Models;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for ProductCartItem.xaml
    /// </summary>
    public partial class ProductCartItem : Border
    {
        public ProductCartItem(DeliveryCartModel cartModel)
        { 
            InitializeComponent();
            DataContext = cartModel;
        }
    }
}
