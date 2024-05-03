using HandyControl.Data;
using HandyControl.Tools.Command;
using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Customs.Panels;
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

        public RelayCommand<FunctionEventArgs<object>> SwitchItemCmd => new(SwitchItem);

        private void SwitchItem(FunctionEventArgs<object> info)
        {
            Grid[] panels = { ProductPanel, CategoryPanel, DashboardPanel, SupplierPanel, StockPanel, DeliveryPanel, AuditPanel,
                            ExpensesPanel, POSPanel, TransactionPanel, UsersPanel, GenSettingsPanel, POSSettingsPanel, AuditTrailPanel };
            foreach(Grid panel in panels)
            {
                panel.Visibility = Visibility.Collapsed;
            }
        }

        public RelayCommand<string> SelectCmd => new(Select);

        private void Select(string header)
        {
            switch (header.ToLower())
            {
                case "dashboard":
                    DashboardPanel.Visibility = Visibility.Visible;
                    break;
                case "point of sale":
                    POSPanel.Visibility = Visibility.Visible;
                    break;
                case "products":
                    ProductPanel.Visibility = Visibility.Visible;
                    break;
                case "categories":
                    CategoryPanel.Visibility = Visibility.Visible;
                    break;
                case "suppliers":
                    SupplierPanel.Visibility = Visibility.Visible;
                    break;
                case "stocks":
                    StockPanel.Visibility = Visibility.Visible;
                    break;
                case "deliveries":
                    DeliveryPanel.Visibility = Visibility.Visible;
                    break;
                case "transaction report":
                    TransactionPanel.Visibility = Visibility.Visible;
                    break;
                case "expenses report":
                    ExpensesPanel.Visibility = Visibility.Visible;
                    break;
                case "users":
                    UsersPanel.Visibility = Visibility.Visible;
                    break;
                case "general settings":
                    GenSettingsPanel.Visibility = Visibility.Visible;
                    break;
                case "pos settings":
                    POSSettingsPanel.Visibility = Visibility.Visible;
                    break;
                case "audit trail":
                    AuditTrailPanel.Visibility = Visibility.Visible;
                    break;
            }
        }

        public void UpdateAll()
        {
            ViewModelLocator loc = new();
            ((CategoryPanelViewModel)CategoryPanel.DataContext).DataList = loc.DService.GetCategoryPanelList();
            ((ProductPanelViewModel)ProductPanel.DataContext).DataList = loc.DService.GetProductList();
            ((SupplierPanelViewModel)SupplierPanel.DataContext).DataList = loc.DService.GetSupplierList();
            ((StocksPanelViewModel)StockPanel.DataContext).DataList = loc.DService.GetStocksList();
            ((UsersPanelViewModel)UsersPanel.DataContext).DataList = loc.DService.GetUsersList();
            ((TransactionPanelViewModel)TransactionPanel.DataContext).DataList = loc.DService.GetTransactionList();
            ((DeliveryPanelViewModel)DeliveryPanel.DataContext).DataList = loc.DService.GetDeliveryList();
            ((AuditTrailPanelViewModel)AuditPanel.DataContext).DataList = loc.DService.GetAuditLogList();
            ((POSPanelViewModel)POSPanel.DataContext).CustomerList = loc.DService.GetCustomerList();

        }
    }
}
