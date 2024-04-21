using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows.Markup;
using wrcaysalesinventory.Data.Models;

namespace wrcaysalesinventory.ViewModels
{
    public class UsersDialogViewModel : ViewModelBase
    {

        public UserModel Model { get; set; } = new();

        private string _firstNameError;
        public string FirstNameError { get => _firstNameError; set => Set(ref _firstNameError, value); }

        private string _lastNameError;
        public string LastNameError { get => _lastNameError; set =>Set(ref _lastNameError, value); }

        private string _phoneError;
        public string PhoneError { get => _phoneError; set => Set(ref _phoneError, value); }

        private string _addressError;
        public string AddressError { get => _addressError; set => Set(ref _addressError, value);  }

        private string _userNameError;
        public string UserNameError { get => _userNameError; set => Set(ref _userNameError, value); }

        private string _roleError;
        public string RoleError { get => _roleError; set => Set(ref _roleError, value); }

        private string _passwordError;
        public string PasswordError { get => _passwordError; set => Set(ref _passwordError, value); }

        public RelayCommand<UsersDialogViewModel> ValidateVM => new(ValidateModel);

        private void ValidateModel(UsersDialogViewModel vm)
        {
            
        }


    }
}
