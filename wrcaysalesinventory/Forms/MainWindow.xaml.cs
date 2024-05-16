using HandyControl.Data;
using HandyControl.Tools.Command;
using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Customs.Panels;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.ViewModels;
using wrcaysalesinventory.ViewModels.PanelViewModes;
using Window = HandyControl.Controls.Window;

namespace wrcaysalesinventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        public void UpdateAll()
        {
            ViewModelLocator loc = new();
            ((CategoryPanelViewModel)CategoryPanel.DataContext).DataList = loc.DService.GetCategoryPanelList();
            ((ProductPanelViewModel)ProductPanel.DataContext).DataList = loc.DService.GetProductList();
            ((ProductPanelViewModel)ProductPanel.DataContext).CategoryDataList = loc.DService.GetCategoryPanelList();
            ((SupplierPanelViewModel)SupplierPanel.DataContext).DataList = loc.DService.GetSupplierList();
            ((StocksPanelViewModel)StockPanel.DataContext).DataList = loc.DService.GetStocksList();
            ((UsersPanelViewModel)UsersPanel.DataContext).DataList = loc.DService.GetUsersList();
            ((TransactionPanelViewModel)TransactionPanel.DataContext).DataList = loc.DService.GetTransactionList();
            ((DeliveryPanelViewModel)DeliveryPanel.DataContext).DataList = loc.DService.GetDeliveryList();
            ((AuditTrailPanelViewModel)AuditPanel.DataContext).DataList = loc.DService.GetAuditLogList();
            ((POSPanelViewModel)PointOfSalePanel.DataContext).Header = new TransactionHeaderModel()
            {
                VAT = GlobalData.Config.TransactionVAT
            };
            ((POSPanelViewModel)PointOfSalePanel.DataContext).DataList = loc.DService.GetStocksList();
            ((POSPanelViewModel)PointOfSalePanel.DataContext).CustomerList = loc.DService.GetCustomerList();
            ((DashboardViewModel)DashboardPanel.DataContext).GenerateNotifok();
            ((DashboardViewModel)DashboardPanel.DataContext).GenerateSeries();
        }
    }
}
