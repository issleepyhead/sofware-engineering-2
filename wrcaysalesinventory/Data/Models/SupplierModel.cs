namespace wrcaysalesinventory.Data.Models
{
    public class SupplierModel
    {
        public string ID { get; set; }
        public string SupplierName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string DateAdded { get; set; }
        public string Status { get; set; }
        public string DateUpdated { get; set; }
        public string StatusColor { get => Status.ToLower() == "active" ? "Green" : "Red";  }
    }
}
