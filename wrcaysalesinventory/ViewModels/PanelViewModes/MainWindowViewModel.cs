using GalaSoft.MvvmLight;
using HandyControl.Tools.Command;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using wrcaysalesinventory.Data.Classes;
using wrcaysalesinventory.Data.DataSet;
using wrcaysalesinventory.Forms;

namespace wrcaysalesinventory.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public RelayCommand<object> OpenReportCmd => new(ReportCommand);
        private void ReportCommand(object obj)
        {

        }
    } 
}
