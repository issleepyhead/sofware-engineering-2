using GalaSoft.MvvmLight.Ioc;
using HandyControl.Controls;
using HandyControl.Themes;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.ViewModels;

namespace wrcaysalesinventory.Customs.Panels
{
    /// <summary>
    /// Interaction logic for DashboardPanel.xaml
    /// </summary>
    public partial class DashboardPanel : Grid
    {
        public DashboardPanel()
        {
            InitializeComponent();
            SimpleIoc.Default.Register<DashboardViewModel>();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(ThemeManager.Current.ApplicationTheme == ApplicationTheme.Dark)
            {
                GlobalData.Config.Theme = ApplicationTheme.Light;
            } else
            {
                GlobalData.Config.Theme = ApplicationTheme.Dark;
            }
            ThemeManager.Current.ApplicationTheme = GlobalData.Config.Theme;
            GlobalData.Save();
        }
    }
}
