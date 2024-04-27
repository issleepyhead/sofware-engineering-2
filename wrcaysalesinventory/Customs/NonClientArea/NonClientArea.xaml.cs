using GalaSoft.MvvmLight;
using HandyControl.Themes;
using HarfBuzzSharp;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            else if(tbtn.IsChecked == false)
            {
                GlobalData.Config.Theme = ApplicationTheme.Light;
                isDark = false;
            }
            ThemeManager.Current.ApplicationTheme = GlobalData.Config.Theme;
            GlobalData.Save();
        }

        private bool isDark = GlobalData.Config.Theme == ApplicationTheme.Dark;
        public bool IsDarkTheme { get => isDark; set => isDark = value;}

    }
}
