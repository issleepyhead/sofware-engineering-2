using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wrcaysalesinventory.Customs.Dialogs;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class GenSettingsPanelViewModel
    {
        public RelayCommand<object> OpenKeyBinding => new(OpenKeyBindingCmd);
        private void OpenKeyBindingCmd(object obj)
        {
            KeyBindingDialog keyBindingDialog = new();
            Dialog.Show(keyBindingDialog);
        }
    }
}
