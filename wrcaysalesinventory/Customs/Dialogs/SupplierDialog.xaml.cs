using System.Windows.Controls;
using wrcaysalesinventory.Resources.Langs;
using wrcaysalesinventory.ViewModels;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for SupplierDialog.xaml
    /// </summary>
    public partial class SupplierDialog : Border
    {
        public SupplierDialog(SupplierDialogViewModel model = null)
        {
            InitializeComponent();
            if(model != null)
            {
                AddButton.Content = Lang.LabelUpdate;
            } else
            {
                DeleteButton.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
