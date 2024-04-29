using Window = HandyControl.Controls.Window;

namespace wrcaysalesinventory.Forms
{
    /// <summary>
    /// Interaction logic for ReportPreview.xaml
    /// </summary>
    public partial class ReportPreview : Window
    {
        public ReportPreview()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ReportViewer.LocalReport.Refresh();
            ReportViewer.RefreshReport();
        }
    }
}
