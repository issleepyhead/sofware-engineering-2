﻿#pragma checksum "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "F38A46618505917B402275345B96B4F9D2BFB0B3CEDCA6790D24B758A222E0B3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Expression.Media;
using HandyControl.Expression.Shapes;
using HandyControl.Interactivity;
using HandyControl.Media.Animation;
using HandyControl.Media.Effects;
using HandyControl.Properties.Langs;
using HandyControl.Themes;
using HandyControl.Tools;
using HandyControl.Tools.Converter;
using HandyControl.Tools.Extension;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Resources.Langs;


namespace wrcaysalesinventory.Customs.Dialogs {
    
    
    /// <summary>
    /// DeliveryCartDialog
    /// </summary>
    public partial class DeliveryCartDialog : System.Windows.Controls.Border, System.Windows.Markup.IComponentConnector {
        
        
        #line 46 "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button CloseBtn;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal HandyControl.Controls.SearchBar SearchTextBox;
        
        #line default
        #line hidden
        
        
        #line 80 "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ReferenceTextBox;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal HandyControl.Controls.ComboBox SupplierNameComboBox;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid ProductDataGridView;
        
        #line default
        #line hidden
        
        
        #line 160 "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button AddButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/wrcaysalesinventory;component/customs/dialogs/deliverycartdialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 15 "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml"
            ((wrcaysalesinventory.Customs.Dialogs.DeliveryCartDialog)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Border_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CloseBtn = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.SearchTextBox = ((HandyControl.Controls.SearchBar)(target));
            
            #line 73 "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml"
            this.SearchTextBox.SearchStarted += new System.EventHandler<HandyControl.Data.FunctionEventArgs<string>>(this.SearchTextBox_SearchStarted);
            
            #line default
            #line hidden
            return;
            case 4:
            this.ReferenceTextBox = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.SupplierNameComboBox = ((HandyControl.Controls.ComboBox)(target));
            return;
            case 6:
            this.ProductDataGridView = ((System.Windows.Controls.DataGrid)(target));
            
            #line 122 "..\..\..\..\Customs\Dialogs\DeliveryCartDialog.xaml"
            this.ProductDataGridView.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.DataGrid_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.AddButton = ((System.Windows.Controls.Button)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

