using System.Windows.Controls;
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
    }
}
