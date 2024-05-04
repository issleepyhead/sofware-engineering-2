using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Properties.Langs;
using wrcaysalesinventory.ViewModels;

namespace wrcaysalesinventory.Customs.Dialogs
{

    public partial class ProductDialog : Border
    {
        public ProductDialog(ProductModel model = null)
        {
            InitializeComponent();
            //if (model != null)
            //{
            //    ((ProductDialogViewModel)DataContext).Model = model;
            //    AddButton.Content = Lang.LabelUpdate;
            //}
            //else
            //{
            //    DeleteButton.Visibility = Visibility.Collapsed;
            //    ((ProductDialogViewModel)DataContext).Model.AllowDecimal = false;
            //}
            
        }

    }
}
