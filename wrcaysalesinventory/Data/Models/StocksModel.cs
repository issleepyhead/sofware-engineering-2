namespace wrcaysalesinventory.Data.Models
{
    public class StocksModel
    {
        public string ID { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string StatusName { get; set; }
        public string StoksUnit { get; set; }
        public string Stocks { get; set; }
        public string Sold { get; set; }
        public string Cost { get; set; }
        public string Defective { get; set; }
        public bool AllowedDecimal { get; set; }
        public string StatusColor { get => StatusName.ToLower() == "active" ? "Green" : "Red"; }
    }
}
