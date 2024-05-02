using HandyControl.Controls;
using System.Windows.Controls;
using System.Windows.Documents;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace wrcaysalesinventory.Forms
{
    /// <summary>
    /// Interaction logic for ReceiptReport.xaml
    /// </summary>
    public partial class ReceiptReport : Window
    {
           
        public ReceiptReport(FixedDocumentSequence fv)
        {
            InitializeComponent();
            DViewer.Document = fv;
            DViewer.FitToHeight();
            DViewer.FitToWidth();
            DViewer.Print();
        }
    }
}
