using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for NoteDialog.xaml
    /// </summary>
    public partial class NoteDialog : Border
    {
        public NoteDialog(TransactionHeaderModel vm = null)
        {
            InitializeComponent();
            DataContext = vm ?? new TransactionHeaderModel();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WinHelper.CloseDialog(CloseBtn);
        }
    }
}
