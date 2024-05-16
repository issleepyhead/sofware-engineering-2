using GalaSoft.MvvmLight;
using HandyControl.Controls;
using HandyControl.Tools.Command;
using HandyControl.Tools.Extension;
using Microsoft.Reporting.WinForms;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Customs.Panels;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.DataSet;
using wrcaysalesinventory.Forms;

namespace wrcaysalesinventory.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IUpdateData
    {
        private MainWindow _mainWin;
        private List<object> panels = new();
        public MainWindowViewModel()
        {
            _mainWin = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        }

        public void UpdateData()
        {
            throw new System.NotImplementedException();
        }

        public RelayCommand<object> LoeaderView => new(LoadedCommand);
        private void LoadedCommand(object obj)
        {
            _mainWin = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            panels.Add(_mainWin.DashboardPanel);
            panels.Add(_mainWin.ProductPanel);
            panels.Add(_mainWin.CategoryPanel);
            panels.Add(_mainWin.SupplierPanel);
            panels.Add(_mainWin.StockPanel);
            panels.Add(_mainWin.DeliveryPanel);
            panels.Add(_mainWin.ExpensesPanel);
            panels.Add(_mainWin.AuditPanel);
            panels.Add(_mainWin.PointOfSalePanel);
            panels.Add(_mainWin.TransactionPanel);
            panels.Add(_mainWin.UsersPanel);
            panels.Add(_mainWin.GenSettingsPanel);
            panels.Add(_mainWin.POSSettingsPanel);

            if((UserPreviledges)GlobalData.Config.RoleID == UserPreviledges.Admin)
            {
                _mainWin.ReportsPanelButton.Visibility = Visibility.Collapsed;
            } else if((UserPreviledges)GlobalData.Config.RoleID == UserPreviledges.Staff)
            {
                _mainWin.ReportsPanelButton.Visibility = Visibility.Collapsed;
                _mainWin.MaintenancePanelButton.Visibility = Visibility.Collapsed;
                _mainWin.DeliveryButton.Visibility = Visibility.Collapsed;
                _mainWin.AuditTrailBackupButton.Visibility = Visibility.Collapsed;
                //_mainWin.DatababseBackupButton.Visibility = Visibility.Collapsed;
                _mainWin.DashboardButton.Visibility = Visibility.Collapsed;
                //_mainWin.GeneralSettingsButton.Visibility = Visibility.Collapsed; 
            }
        }

        public RelayCommand<object> SelectCmd => new(Select);
        private void Select(object obj)
        {
            foreach (object grid in panels)
                ((Grid)grid).Visibility = Visibility.Collapsed;

            ((Grid)obj).Visibility = Visibility.Visible;
        }

        public RelayCommand<object> SelectInputBinding => new(SelectInputBindingCommand);
        private void SelectInputBindingCommand(object obj)
        {
            Growl.Info("F5 PRESSED!");
        }
    } 
}
