namespace wrcaysalesinventory.Data.Models
{
    public class ProductModel
    {
        public string ID { get; set; }
        public string CategoryID { get; set; }
        public string StatusID { get; set; }
        public string CategoryName { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ProductPrice { get; set; }
        public string ProductUnit { get; set; }
        public string ProductCost { get; set; }
        public string DateAdded { get; set; }
        public string DateUpdated { get; set; }
        public bool NotAllowDecimal { get; set; } = true;
        public string   StatusName { get; set; }
    }
}
