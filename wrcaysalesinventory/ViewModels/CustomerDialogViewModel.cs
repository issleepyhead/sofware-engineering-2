using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wrcaysalesinventory.ViewModels
{
    public class CustomerDialogViewModel : ViewModelBase
    {
        private string _fullnameError;
        private string _phoneError;
        private string _emailError;


        public string EmailError { get => _emailError; set => Set(ref _emailError, value); }
        public string FullNameError { get => _fullnameError; set => Set(ref _fullnameError, value); }
        public string PhoneError { get => _phoneError; set => Set(ref _phoneError, value); }



    }
}
