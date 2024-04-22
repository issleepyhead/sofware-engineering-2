using HandyControl.Controls;
using HandyControl.Tools.Command;
using wrcaysalesinventory.Customs.Dialogs;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class UsersPanelViewModel
    {
        public RelayCommand<object> OpenUser => new(OpenUserDialog);
        private void OpenUserDialog(object obj) => Dialog.Show(new UserDialog());
    }
}
