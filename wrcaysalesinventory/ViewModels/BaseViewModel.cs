using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace wrcaysalesinventory.ViewModels
{
    public class BaseViewModel<T> : ViewModelBase
    {
        private ObservableCollection<T> _dataList;

        public ObservableCollection<T> DataList
        {
            get => _dataList;
            set => Set(ref _dataList, value);
        }
    }
}
