using GalaSoft.MvvmLight.Ioc;
using System;
using System.Windows;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels
{
    public class ViewModelLocator
    {
        [PreferredConstructor]
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<DataService>();
            SimpleIoc.Default.Register<DashboardViewModel>();
        }


        public static ViewModelLocator Instance = new Lazy<ViewModelLocator>(() =>
            Application.Current.TryFindResource("Locator") as ViewModelLocator).Value;

        public DashboardViewModel DashboardView = SimpleIoc.Default.GetInstance<DashboardViewModel>();

    }
}
