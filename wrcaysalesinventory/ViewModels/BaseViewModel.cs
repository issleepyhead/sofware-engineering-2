using GalaSoft.MvvmLight;
using System.Collections.Generic;

namespace wrcaysalesinventory.ViewModels
{
    public class BaseViewModel<T> : ViewModelBase
    {
        private IList<T> _dataList;

        public IList<T> DataList
        {
            get => _dataList;
            set => Set(ref _dataList, value);
        }
    }
}
