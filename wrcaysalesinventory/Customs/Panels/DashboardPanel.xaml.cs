using System.Windows.Controls;
using wrcaysalesinventory.ViewModels;

namespace wrcaysalesinventory.Customs.Panels
{
    /// <summary>
    /// Interaction logic for DashboardPanel.xaml
    /// </summary>
    public partial class DashboardPanel : UserControl
    {
        public DashboardPanel()
        {
            InitializeComponent();
            DataContext = new DashboardViewModel();
        }
    }
}
