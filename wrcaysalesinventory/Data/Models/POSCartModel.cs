using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wrcaysalesinventory.Data.Models
{
    public class POSCartModel
    {
        public string ID { get; set; } 
        public string ProductName { get; set; }
        public string Quantity { get; set; } = "0";
        public string Cost { get; set; } = "0";
        public string Stocks { get; set; } = "0";
        public string Total {
            get
            {
                try
                {
                    if (double.Parse(Quantity) < 1)
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
        public bool AllowedDecimal { get; set; } = false;
    }
}
