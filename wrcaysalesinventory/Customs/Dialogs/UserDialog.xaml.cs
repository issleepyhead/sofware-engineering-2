using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Resources.Langs;
using wrcaysalesinventory.ViewModels;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for UserDialog.xaml
    /// </summary>
    public partial class UserDialog : Border
    {
        public UserDialog(UserModel model = null)
        {
            InitializeComponent();
            if(model != null)
            {
                ((UsersDialogViewModel)DataContext).Model = model;
                AddButton.Content = Lang.LabelUpdate;
            } else
            {
                DeleteButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
