using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for DiscountDialog.xaml
    /// </summary>
    public partial class DiscountDialog : Border
    {
        public DiscountDialog(POSPanelViewModel vm = null)
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (Regex.IsMatch(DiscountTextBox.Text, "^(\\d+)?\\.?(\\d+)$"))
                {
                    if (double.Parse(DiscountTextBox.Text) > 20)
                    {
                        ((POSPanelViewModel)DataContext).DiscountError = "Discount can't be higher than 20%.";
                        ((POSPanelViewModel)DataContext).Discount = "20";
                        return;
                    }
                ((POSPanelViewModel)DataContext).Discount = DiscountTextBox.Text;
                    WinHelper.CloseDialog(CloseBtn);
                }
                else
                {
                    ((POSPanelViewModel)DataContext).DiscountError = "Invalid Discount.";
                    ((POSPanelViewModel)DataContext).Discount = "0";
                }
                ((POSPanelViewModel)DataContext).ValueChanged();
            } catch
            {
                Debug.WriteLine("Invalid Format");
            }
        }
    }
}
