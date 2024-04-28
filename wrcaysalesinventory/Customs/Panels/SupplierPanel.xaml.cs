using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Panels
{
    /// <summary>
    /// Interaction logic for SupplierPanel.xaml
    /// </summary>
    public partial class SupplierPanel : Grid
    {
        public SupplierPanel()
        {
            InitializeComponent();
            WinHelper.PaginationPageCount(PagerPagination, ((SupplierPanelViewModel)DataContext).TotalData);
        }

        private void PagerPagination_PageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            ((SupplierPanelViewModel)DataContext).PageUpdated(e.Info);
        }
    }
}
