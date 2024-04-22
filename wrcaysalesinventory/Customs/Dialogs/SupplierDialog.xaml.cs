using System.Windows.Controls;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Resources.Langs;
using wrcaysalesinventory.ViewModels;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for SupplierDialog.xaml
    /// </summary>
    public partial class SupplierDialog : Border
    {
        public SupplierDialog(SupplierModel model = null)
        {
            InitializeComponent();
            if(model != null)
            {
                ((SupplierDialogViewModel)DataContext).Model = model;
                AddButton.Content = Lang.LabelUpdate;
            } else
            {
                DeleteButton.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
    }
}
