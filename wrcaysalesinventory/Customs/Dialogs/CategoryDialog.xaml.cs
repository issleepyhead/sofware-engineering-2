using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Properties.Langs;
using wrcaysalesinventory.ViewModels;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for CategoryDialog.xaml
    /// </summary>
    public partial class CategoryDialog : Border
    {
        private readonly CategoryModel _categoryModel;
        public CategoryDialog(CategoryModel category = null)
        {
            InitializeComponent();
            if (category != null)
            {
                _categoryModel = category;
                AddButton.Content = Lang.LabelUpdate;
            }
            else
            {
                _categoryModel = new();
                DeleteButton.Visibility = Visibility.Collapsed;
            }
            ((CategoryDialogViewModel)DataContext).Model = _categoryModel;
        }
    }
}
