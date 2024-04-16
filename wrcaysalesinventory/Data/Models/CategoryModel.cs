using GalaSoft.MvvmLight;

namespace wrcaysalesinventory.Data.Models
{
    public class CategoryModel
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string DateAdded { get; set; }
        public string DateUpdated { get; set; }
    }
}
