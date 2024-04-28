using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Panels
{
    /// <summary>
    /// Interaction logic for CategoriesPanel.xaml
    /// </summary>
    public partial class CategoriesPanel : Grid
    {
        public CategoriesPanel()
        {
            InitializeComponent();
            WinHelper.PaginationPageCount(PagerPagination, ((CategoryPanelViewModel)DataContext).TotalData);
        }

        private void Pagination_PageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            ((CategoryPanelViewModel)DataContext).PageUpdated(e.Info);
        }
    }
}
