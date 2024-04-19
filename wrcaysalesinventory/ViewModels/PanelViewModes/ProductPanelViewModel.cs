using GalaSoft.MvvmLight;
using HandyControl.Controls;
using HandyControl.Tools.Command;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class ProductPanelViewModel : BaseViewModel<ProductModel>
    {
        private DataService _dataService;
        public ProductPanelViewModel(DataService dataService)
        {
            DataList = dataService.GetProductList();
        }

        public RelayCommand<object> OpenDialog => new(OpenProductDialog);

        private void OpenProductDialog(object obj) => Dialog.Show(new ProductDialog());
    }
}
