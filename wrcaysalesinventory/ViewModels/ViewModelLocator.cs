using GalaSoft.MvvmLight.Ioc;
using wrcaysalesinventory.Services;

namespace wrcaysalesinventory.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<DataService>();
        }

    }
}
