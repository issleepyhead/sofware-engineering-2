using HandyControl.Controls;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Panels
{
    /// <summary>
    /// Interaction logic for POSPanel.xaml
    /// </summary>
    public partial class POSPanel : Grid
    {
        public POSPanel()
        {
            InitializeComponent();

        }

        private void NumericUpDown_ValueChanged(object sender, HandyControl.Data.FunctionEventArgs<double> e)
        {
            POSPanelViewModel pmodel = (POSPanelViewModel)DataContext;
            pmodel.ValueChanged();
        }



    }


}
