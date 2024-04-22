using HandyControl.Controls;
using HandyControl.Tools.Command;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;
using System.Windows.Controls;

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

        public RelayCommand<object> SelectedCommand => new(SelectionChanged);
        private void SelectionChanged(object obj)
        {
            DataGrid pdataGrid;
            if (obj.GetType() == typeof(DataGrid))
            {
                 pdataGrid = (DataGrid)obj;
                if(pdataGrid.SelectedItems.Count > 0)
                {
                    ProductModel model = (ProductModel)pdataGrid.SelectedItem;
                    var d = new ProductDialog(model);
                    ((ProductDialogViewModel)d.DataContext).BTN = d.CloseBtn;
                    Dialog.Show(d);
                    pdataGrid.ItemsSource = pdataGrid.ItemsSource;
                }
            }
        }
    }
}
