using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Panels
{
    /// <summary>
    /// Interaction logic for DeliveryPanel.xaml
    /// </summary>
    public partial class DeliveryPanel : Grid
    {
        public DeliveryPanel()
        {
            InitializeComponent();
            WinHelper.PaginationPageCount(PagerPagination, ((DeliveryPanelViewModel)DataContext).TotalData);
        }

        private void PagerPagination_PageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            ((DeliveryPanelViewModel)DataContext).PageUpdated(e.Info);
        }
    }
}
