using HandyControl.Controls;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.ViewModels;
using wrcaysalesinventory.ViewModels.PanelViewModes;
using ComboBox = HandyControl.Controls.ComboBox;

namespace wrcaysalesinventory.Customs.Panels
{
    /// <summary>
    /// Interaction logic for ProductPanel.xaml
    /// </summary>
    public partial class ProductPanel : Grid
    {
        public ProductPanel()
        {
            InitializeComponent();
            WinHelper.PaginationPageCount(PagerPagination, ((ProductPanelViewModel)DataContext).TotalData);
        }

        private void PagerPagination_PageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            ((ProductPanelViewModel)DataContext).PageUpdated(e.Info);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            if (cmb.SelectedIndex != -1 && cmb.SelectedValue != "-1")
            {
                ((ProductPanelViewModel)DataContext).AllData = ((ProductPanelViewModel)DataContext).DataService.GetProductByCategoryList(cmb.SelectedValue.ToString());
            } else
            {
                ((ProductPanelViewModel)DataContext).AllData = ((ProductPanelViewModel)DataContext).DataService.GetProductList();
            }
            WinHelper.PaginationPageCount(PagerPagination, ((ProductPanelViewModel)DataContext).TotalData);
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;
            if (cmb.SelectedIndex != -1 && cmb.SelectedValue != "-1")
            {
                ((ProductPanelViewModel)DataContext).AllData = ((ProductPanelViewModel)DataContext).DataService.GetProductByStatusList(cmb.SelectedValue.ToString());
            }
            else
            {
                ((ProductPanelViewModel)DataContext).AllData = ((ProductPanelViewModel)DataContext).DataService.GetProductList();
            }
            WinHelper.PaginationPageCount(PagerPagination, ((ProductPanelViewModel)DataContext).TotalData);
        }
    }
}
