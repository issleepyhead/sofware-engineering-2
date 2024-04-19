using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HandyControl.Controls;
using wrcaysalesinventory.Customs.Dialogs;
using wrcaysalesinventory.Data.Models;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels.PanelViewModes
{
    public class SupplierPanelViewModel : BaseViewModel<SupplierModel>
    {
        public SupplierPanelViewModel(DataService dataService)
        {
            DataList = dataService.GetSupplierList();
        }

        private string _supplierNameError;
        public string SupplierNameError { get => _supplierNameError; set => Set(ref _supplierNameError, value); }

        private string _firstNameError;
        public string FirstNameError { get => _firstNameError; set => Set(ref _firstNameError, value); }

        private string _lastNameError;
        public string LastNameError { get => _lastNameError; set => Set(ref _lastNameError, value); }

        private string _cityError;
        public string CityError { get => _cityError; set => Set(ref _cityError, value); }

        private string _countryError;
        public string CoutryError { get => _countryError; set => Set(ref _countryError, value); }

        private string _addressError;
        public string AddressError { get => _addressError; set => Set(ref _addressError, value); }

        private string _phoneError;
        public string PhoneError { get => _phoneError; set => Set(ref _phoneError, value); }

        public RelayCommand<object> OpenSupplier => new(OpenSupplierDialog);
        private void OpenSupplierDialog(object obj) => Dialog.Show(new SupplierDialog());
    }
}
