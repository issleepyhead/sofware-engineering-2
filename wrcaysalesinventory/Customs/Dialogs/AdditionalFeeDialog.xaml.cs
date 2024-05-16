using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for AdditionalFeeDialog.xaml
    /// </summary>
    public partial class AdditionalFeeDialog : Border
    {
        public AdditionalFeeDialog(POSPanelViewModel vm = null)
        {
            InitializeComponent();
            DataContext = vm;
        }

        //private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        //{
        //    ((POSPanelViewModel)DataContext).ValueChanged();
        //    WinHelper.CloseDialog(CloseBtn);
        //}
    }
}
