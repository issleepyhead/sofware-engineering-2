﻿using GalaSoft.MvvmLight.Ioc;
using System;
using System.Diagnostics;
using System.Windows;
using wrcaysalesinventory.Services;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<DataService>();
            SimpleIoc.Default.Register<DashboardViewModel>();
            SimpleIoc.Default.Register<MainWindowViewModel>();
            SimpleIoc.Default.Register<CategoryDialogViewModel>();
            SimpleIoc.Default.Register<ProductDialogViewModel>();
            SimpleIoc.Default.Register<ProductPanelViewModel>();
            SimpleIoc.Default.Register<CategoryPanelViewModel>();
            SimpleIoc.Default.Register<SupplierPanelViewModel>();
            SimpleIoc.Default.Register<SupplierDialogViewModel>();
            SimpleIoc.Default.Register<UsersDialogViewModel>();
            SimpleIoc.Default.Register<DeliveryPanelViewModel>();
            SimpleIoc.Default.Register<UsersPanelViewModel>();
            SimpleIoc.Default.Register<DeliveryCartDialogViewModel>();
            SimpleIoc.Default.Register<StocksPanelViewModel>();
            SimpleIoc.Default.Register<POSPanelViewModel>();
            SimpleIoc.Default.Register<TransactionPanelViewModel>();
            SimpleIoc.Default.Register<POSSettingsPanelViewModel>();
            SimpleIoc.Default.Register<POSSettingsReceiptDialogViewModel>();
        }

        public static ViewModelLocator Instance = new Lazy<ViewModelLocator>(() =>
                 Application.Current.TryFindResource("Locator") as ViewModelLocator).Value;

        public DashboardViewModel DashboardView => SimpleIoc.Default.GetInstance<DashboardViewModel>();
        public MainWindowViewModel MainWindowView => SimpleIoc.Default.GetInstance<MainWindowViewModel>();
        public CategoryDialogViewModel CategoryDialogView => SimpleIoc.Default.GetInstance<CategoryDialogViewModel>();
        public ProductDialogViewModel ProductDialogView => new(SimpleIoc.Default.GetInstance<DataService>());
        public ProductPanelViewModel ProductPanelView => SimpleIoc.Default.GetInstance<ProductPanelViewModel>();
        public CategoryPanelViewModel CategoryPanelView => SimpleIoc.Default.GetInstance<CategoryPanelViewModel>();
        public SupplierPanelViewModel SupplierPanelView => SimpleIoc.Default.GetInstance<SupplierPanelViewModel>();
        public SupplierDialogViewModel SupplierDialoglView => SimpleIoc.Default.GetInstance<SupplierDialogViewModel>();
        public UsersDialogViewModel UsersDialoglView => new(SimpleIoc.Default.GetInstance<DataService>());
        public DeliveryPanelViewModel DeliveryPanellView => SimpleIoc.Default.GetInstance<DeliveryPanelViewModel>();
        public UsersPanelViewModel UserPanelView => SimpleIoc.Default.GetInstance<UsersPanelViewModel>();
        public DeliveryCartDialogViewModel DeliveryCartDialoglView => new(SimpleIoc.Default.GetInstance<DataService>());
        public DataService DService => SimpleIoc.Default.GetInstance<DataService>();

        public StocksPanelViewModel StocksPanelView => new(SimpleIoc.Default.GetInstance<DataService>());
        public POSPanelViewModel POSPanelView => new(SimpleIoc.Default.GetInstance<DataService>());
        public TransactionPanelViewModel TransactionlView => new(SimpleIoc.Default.GetInstance<DataService>());
        public POSSettingsPanelViewModel POSSettingsView => SimpleIoc.Default.GetInstance<POSSettingsPanelViewModel>();
        public POSSettingsReceiptDialogViewModel POSSettingsReceiptView => SimpleIoc.Default.GetInstance<POSSettingsReceiptDialogViewModel>();
    }
}
