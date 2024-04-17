using GalaSoft.MvvmLight.Ioc;
using System;
using System.Diagnostics;
using System.Windows;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<DataService>();
            SimpleIoc.Default.Register<DashboardViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<CategoryViewModel>();
            SimpleIoc.Default.Register<CategoryDialogViewModel>();
            SimpleIoc.Default.Register<ProductDialogViewModel>();
            
        }

        public static ViewModelLocator Instance = new Lazy<ViewModelLocator>(() =>
                 Application.Current.TryFindResource("Locator") as ViewModelLocator).Value;

        public DashboardViewModel DashboardView => SimpleIoc.Default.GetInstance<DashboardViewModel>();
        public CategoryViewModel CategoryView => SimpleIoc.Default.GetInstance<CategoryViewModel>();
        public MainWindowViewModel MainWindowView => SimpleIoc.Default.GetInstance<MainWindowViewModel>();
        public CategoryDialogViewModel CategoryDialogView => SimpleIoc.Default.GetInstance<CategoryDialogViewModel>();
        public ProductDialogViewModel ProductDialogView => SimpleIoc.Default.GetInstance<ProductDialogViewModel>();

    }
}
