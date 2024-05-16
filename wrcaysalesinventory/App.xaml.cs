using HandyControl.Themes;
using HandyControl.Tools;
using HandyControl.Tools.Extension;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Properties;

namespace wrcaysalesinventory
{
    public partial class App : Application
    {
        #pragma warning disable IDE0052
        private static Mutex AppMutex;
        #pragma warning restore IDE0052

        public App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            AppMutex = new Mutex(true, "wrcaysalesinventory", out var createdNew);

            if (!createdNew)
            {
                var current = Process.GetCurrentProcess();

                foreach (var process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        WinHelper.SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                Shutdown();
            } else
            {
                ApplicationHelper.IsSingleInstance();
                ShutdownMode = ShutdownMode.OnLastWindowClose;
                GlobalData.Init();
                ConfigHelper.Instance.SetLang(GlobalData.Config.Lang);

                ConfigHelper.Instance.SetWindowDefaultStyle();
                ConfigHelper.Instance.SetNavigationWindowDefaultStyle();
                if (GlobalData.Config.Theme != ApplicationTheme.Light)
                {
                    UpdateSkin(GlobalData.Config.Theme);
                }
                if (string.IsNullOrEmpty(Settings.Default.connStr))
                {
                    StartupUri = new System.Uri("Forms/ServerWindow.xaml", System.UriKind.Relative);
                }
                else
                {
                    if (GlobalData.Config.UserID != -1)
                    {
                        StartupUri = new System.Uri("Forms/MainWindow.xaml", System.UriKind.Relative);
                    } else
                   {
                        StartupUri = new System.Uri("Forms/LoginWindow.xaml", System.UriKind.Relative);
                    }
                }
            }
        }


        internal void UpdateSkin(ApplicationTheme theme)
        {
                ThemeAnimationHelper.AnimateTheme(Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(), ThemeAnimationHelper.SlideDirection.Top, 0.3, 1, 0.5);

                ThemeManager.Current.ApplicationTheme = theme;

                ThemeAnimationHelper.AnimateTheme(Application.Current.Windows.OfType<MainWindow>().FirstOrDefault(), ThemeAnimationHelper.SlideDirection.Bottom, 0.3, 0.5, 1);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            GlobalData.Save();
        }
    }
}
