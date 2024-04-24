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
        public UserDialog(UserModel user = null)
        {
            InitializeComponent();
            if (user != null)
            {
                ((UsersDialogViewModel)DataContext).Model = user;
                AddButton.Content = Lang.LabelUpdate;
                DeleteButton.Visibility = Visibility.Visible;
            }
        }
    }
}
