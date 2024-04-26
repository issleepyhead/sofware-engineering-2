﻿using System;
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
        public string Total { get => (double.Parse(Quantity ?? "0") * double.Parse(Cost ?? "0")).ToString() ; }
    }
}
