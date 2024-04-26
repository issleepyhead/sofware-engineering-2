using System.Windows.Controls;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for DiscountDialog.xaml
    /// </summary>
    public partial class DiscountDialog : Border
    {
        public DiscountDialog(POSPanelViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
