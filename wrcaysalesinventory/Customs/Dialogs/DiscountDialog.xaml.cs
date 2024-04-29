using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for DiscountDialog.xaml
    /// </summary>
    public partial class DiscountDialog : Border
    {
        public DiscountDialog(POSPanelViewModel vm = null)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((POSPanelViewModel)DataContext).ValueChanged();
            WinHelper.CloseDialog(CloseBtn);
        }
    }
}
