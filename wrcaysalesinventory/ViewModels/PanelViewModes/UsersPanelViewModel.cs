using HandyControl.Controls;
using HandyControl.Tools.Command;
using System.Windows.Controls;
using System.Windows.Forms;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;
using DataGrid = System.Windows.Controls.DataGrid;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class UsersPanelViewModel : BaseViewModel<UserModel>
    {
        private DataService _dataService;
        public UsersPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            DataList = _dataService.GetUsersList();
        }
        public RelayCommand<object> OpenUser => new(OpenUserDialog);
        private void OpenUserDialog(object obj)
        {
            UserDialog d = new();
            ((UsersDialogViewModel)d.DataContext).BTN = d.CloseBtn;
            Dialog.Show(d);
        }

        public RelayCommand<DataGrid> SelectCommand => new(SelectCmd);
        private void SelectCmd(DataGrid dataGrid)
        {
            if (dataGrid.SelectedItems.Count > 0)
            {
                UserModel model = (UserModel)dataGrid.SelectedItem;
                UserDialog d = new(model);
                ((UsersDialogViewModel)d.DataContext).BTN = d.CloseBtn;
                Dialog.Show(d);
            }
        }
    }
}
