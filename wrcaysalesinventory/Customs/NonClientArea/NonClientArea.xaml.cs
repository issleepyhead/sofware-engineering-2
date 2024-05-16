using GalaSoft.MvvmLight;
using HandyControl.Controls;
using HandyControl.Themes;
using HarfBuzzSharp;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Classes;

namespace wrcaysalesinventory.Customs.NonClientArea
{
    /// <summary>
    /// Interaction logic for NonClientArea.xaml
    /// </summary>
    public partial class NonClientArea : Grid
    {
        public NonClientArea()
        {
            InitializeComponent();
        }



        private void ToggleButton_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton tbtn = (ToggleButton)sender;
            if (tbtn.IsChecked == true)
            {
                GlobalData.Config.Theme = ApplicationTheme.Dark;
                isDark = true;
            }
            else
            {
                GlobalData.Config.Theme = ApplicationTheme.Light;
                isDark = false;
            }
            ThemeManager.Current.ApplicationTheme = GlobalData.Config.Theme;
            GlobalData.Save();
        }

        private bool isDark = GlobalData.Config.Theme == ApplicationTheme.Dark;
        public bool IsDarkTheme { get => isDark; set { isDark = value; } }

        private void ToggleBtn_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            ToggleButton tbtn = (ToggleButton)sender;
            if (tbtn.IsChecked == true)
            {
                GlobalData.Config.Theme = ApplicationTheme.Dark;
                isDark = true;
            }
            else
            {
                GlobalData.Config.Theme = ApplicationTheme.Light;
                isDark = false;
            }
            ThemeManager.Current.ApplicationTheme = GlobalData.Config.Theme;
            GlobalData.Save();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ProfileDialog profile = new();
            Dialog.Show(profile);
        }
    }
}
