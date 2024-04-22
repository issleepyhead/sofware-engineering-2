using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using wrcaysalesinventory.Data.Models;

namespace wrcaysalesinventory.ViewModels
{
    public class DeliveryCartDialogViewModel : ViewModelBase
    {
        private ObservableCollection<ProductModel> deliveryCartModels;
        public ObservableCollection<ProductModel> DeliveryCartList { get => deliveryCartModels; set => Set(ref deliveryCartModels, value);}


    }
}
