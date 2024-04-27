using System.Windows.Controls;
using wrcaysalesinventory.ViewModels.PanelViewModes;

namespace wrcaysalesinventory.Customs.Dialogs
{
    /// <summary>
    /// Interaction logic for NoteDialog.xaml
    /// </summary>
    public partial class NoteDialog : Border
    {
        public NoteDialog(POSPanelViewModel vm = null)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
