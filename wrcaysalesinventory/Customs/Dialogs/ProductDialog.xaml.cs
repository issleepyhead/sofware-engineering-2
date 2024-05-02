﻿using System.Windows;
using System.Windows.Controls;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Resources.Langs;
using wrcaysalesinventory.ViewModels;

namespace wrcaysalesinventory.Customs.Dialogs
{

    public partial class ProductDialog : Border
    {
        public ProductDialog(ProductModel model = null)
        {
            InitializeComponent();
            if (model != null)
            {
                ((ProductDialogViewModel)DataContext).Model = model;
                AddButton.Content = Lang.LabelUpdate;
                allowed.IsChecked = !model.AllowDecimal;
                notallowed.IsChecked = model.AllowDecimal;
            }
            else
            {
                DeleteButton.Visibility = Visibility.Collapsed;
                ((ProductDialogViewModel)DataContext).Model.AllowDecimal = false;
            }
            
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ((ProductDialogViewModel)DataContext).Model.AllowDecimal = false;
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            ((ProductDialogViewModel)DataContext).Model.AllowDecimal = true;
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ((ProductDialogViewModel)DataContext).Model.AllowDecimal = false;
        }

        private void RadioButton_Unchecked_1(object sender, RoutedEventArgs e)
        {
            ((ProductDialogViewModel)DataContext).Model.AllowDecimal = true;
        }
    }
}
