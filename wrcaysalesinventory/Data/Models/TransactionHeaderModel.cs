using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wrcaysalesinventory.Data.Classes;

namespace wrcaysalesinventory.Data.Models
{
    public class TransactionHeaderModel
    {
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string UserID { get; set; }
        public string ReferenceNumber { get; set; }
        public string Note { get; set; }

        public string TotalAmount { get ; set; } = "0";
        public string AdditionalFee { get; set; } = "0";
        public string Discount { get; set; } = "0";
        public string VAT { get; set; } = GlobalData.Config.TransactionVAT;
        public string DateAdded { get; set; }
    }
}
