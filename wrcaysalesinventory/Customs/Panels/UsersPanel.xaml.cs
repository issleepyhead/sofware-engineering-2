using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Panels
{
    public partial class UsersPanel : Grid
    {
        public UsersPanel()
        {
            InitializeComponent();
            WinHelper.PaginationPageCount(PagerPagination, ((UsersPanelViewModel)DataContext).TotalData);
        }

        private void PagerPagination_PageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            ((UsersPanelViewModel)DataContext).PageUpdated(e.Info);
        }
    }
}
