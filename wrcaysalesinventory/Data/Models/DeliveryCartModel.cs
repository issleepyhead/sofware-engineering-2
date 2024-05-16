using System.Windows.Markup;

namespace wrcaysalesinventory.Data.Models
{
    public class DeliveryCartModel
    {
        public string ID { get; set; }
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set;}
        public string StatusName { get; set; }
        public string StatusColor { get => StatusName.ToString() == "active" ? "Green" : "Red"; }
        public bool AllowedDecimal { get; set; } = false;
        public string Cost { get; set; }
        public string TotalDue { get; set; }
        public string Total {
            get
            {
                try
                {
                    if(double.Parse(Quantity) < 1)
                    {
                        return (double.Parse(Quantity) / 1 * double.Parse(Cost)).ToString();
                    }
                    return (double.Parse(Quantity) * double.Parse(Cost)).ToString();
                }
                catch
                {
                    return "0";
                }
            }
        }
    }
}
