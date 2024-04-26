﻿namespace wrcaysalesinventory.Data.Models
{
    public class TransactionModel
    {
        public string ID { get; set; }  
        public string ReferenceNumber { get; set; }  
        public string TotalItems { get; set; }  
        public string TotalCost { get; set; }  
        public string Discount { get; set; }  
        public string AdditionalFee  { get; set; }  
        public string VAT { get; set; }  
        public string DateAdded { get; set; }  
    }
}
