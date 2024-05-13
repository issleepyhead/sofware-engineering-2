using HandyControl.Controls;
using HandyControl.Tools.Command;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;
using ComboBox = HandyControl.Controls.ComboBox;
using DataGrid = System.Windows.Controls.DataGrid;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class UsersPanelViewModel : BaseViewModel<UserModel>
    {
        private DataService _dataService;
        private ObservableCollection<UserModel> _alldata;
        public UsersPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            _alldata = _dataService.GetUsersList();
            DataList = new ObservableCollection<UserModel>(_alldata.Take(30).ToList());
        }

        public int TotalData { get => _alldata.Count; }

        public ObservableCollection<StatusModel> StatusDataList { get => _dataService.GetStatusList(); }

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
                UserDialog d = new();
                ((UsersDialogViewModel)d.DataContext).BTN = d.CloseBtn;
                model.Password = string.Empty;
                ((UsersDialogViewModel)d.DataContext).Model = model;
                Dialog.Show(d);
                dataGrid.SelectedItems.Clear();
                dataGrid.SelectedCells.Clear();
            }
        }

        public RelayCommand<SearchBar> SearchCmd => new(SearchCommand);
        public void SearchCommand(SearchBar searchBar)
        {
            DataList = _dataService.SearchUsersList(string.IsNullOrEmpty(searchBar.Text) ? "%" : searchBar.Text);
        }

        public void PageUpdated(int offset)
        {
            DataList = new ObservableCollection<UserModel>(_alldata.Skip(offset * 30).Take(30).ToList());
        }

        public RelayCommand<object> FilterStatusCmd => new(FilterStatusCommand);
        private void FilterStatusCommand(object obj)
        {
            try
            {
                ComboBox cmbBox = (ComboBox)obj;
                if (cmbBox.SelectedIndex != -1 && cmbBox.SelectedValue.ToString() != "-1")
                {
                    _alldata = _dataService.GetUsersListByStatus(cmbBox.SelectedValue.ToString());
                    DataList = new ObservableCollection<UserModel>(_alldata.Take(30).ToList());
                }
                else
                {
                    _alldata = _dataService.GetUsersList();
                    DataList = new ObservableCollection<UserModel>(_alldata.Take(30).ToList());
                }
            }
            catch
            {

            }

        }
    }
}
