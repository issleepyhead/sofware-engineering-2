using HandyControl.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class CustomerPanelViewModel : BaseViewModel<CustomerModel>
    {
        private ObservableCollection<CustomerModel> _dataList;
        private DataService _dataService;
        public CustomerPanelViewModel(DataService dataService)
        {
            _dataService = dataService;
            _dataList = _dataService.GetCustomerList();
        }


    }
}
