using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using System.Windows.Controls;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class SupplierPanelViewModel : BaseViewModel<SupplierModel>
    {
        public SupplierPanelViewModel(DataService dataService)
        {
            DataList = dataService.GetSupplierList();
        }
        
        public RelayCommand<object> OpenSupplier => new(OpenSupplierDialog);
        private void OpenSupplierDialog(object obj) => Dialog.Show(new SupplierDialog());

        public RelayCommand<object> SelectedCommand => new(SelectionChanged);
        private void SelectionChanged(object obj)
        {
            DataGrid pdataGrid;
            if (obj.GetType() == typeof(DataGrid))
            {
                pdataGrid = (DataGrid)obj;
                if (pdataGrid.SelectedItems.Count > 0)
                {
                    SupplierModel model = (SupplierModel)pdataGrid.SelectedItem;
                    var d = new SupplierDialog(model);
                    ((SupplierDialogViewModel)d.DataContext).BTN = d.Closebtn;
                    Dialog.Show(d);
                    pdataGrid.ItemsSource = pdataGrid.ItemsSource;
                }
            }
        }
    }
}
