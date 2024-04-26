using System.Windows.Controls;
using wrcaysalesinventory.Data.Models;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for POSProductItem.xaml
    /// </summary>
    public partial class POSProductItem : Grid
    {
        public POSProductItem(POSCartModel model)
        {
            InitializeComponent();
            if (model != null)
                DataContext = model;
        }
    }
}
