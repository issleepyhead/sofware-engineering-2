﻿#pragma checksum "..\..\..\..\Customs\NonClientArea\NonClientArea.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "0A605301C14AD260714D564EFA7F5BBA04BAFE582DA31EDF031EE5D279CE5EC3"
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
using System.Windows.Forms.Integration;
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
using wrcaysalesinventory.Customs.NonClientArea;
using wrcaysalesinventory.Properties.Langs;


namespace wrcaysalesinventory.Customs.NonClientArea {
    
    
    /// <summary>
    /// NonClientArea
    /// </summary>
    public partial class NonClientArea : System.Windows.Controls.Grid, System.Windows.Markup.IComponentConnector {
        
        
        #line 42 "..\..\..\..\Customs\NonClientArea\NonClientArea.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.ToggleButton ToggleBtn;
        
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
            System.Uri resourceLocater = new System.Uri("/wrcaysalesinventory;component/customs/nonclientarea/nonclientarea.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Customs\NonClientArea\NonClientArea.xaml"
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
            this.ToggleBtn = ((System.Windows.Controls.Primitives.ToggleButton)(target));
            
            #line 43 "..\..\..\..\Customs\NonClientArea\NonClientArea.xaml"
            this.ToggleBtn.Checked += new System.Windows.RoutedEventHandler(this.ToggleButton_Checked);
            
            #line default
            #line hidden
            
            #line 43 "..\..\..\..\Customs\NonClientArea\NonClientArea.xaml"
            this.ToggleBtn.Unchecked += new System.Windows.RoutedEventHandler(this.ToggleBtn_Unchecked);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 51 "..\..\..\..\Customs\NonClientArea\NonClientArea.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

