using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
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
    }
}
