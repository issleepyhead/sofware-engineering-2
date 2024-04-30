﻿using HandyControl.Themes;
using System;
using System.Collections.Generic;
using System.Windows.Documents;

namespace wrcaysalesinventory.Data.Classes
{
    public class AppConfig
    {
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}AppConfig.json";

        public string Lang { get; set; } = "en-US";

        public int UserRole { get; set; } = 3;

        public ApplicationTheme Theme { get; set; }
        public string TransactionVAT { get; set; } = "0";
        public string TransactionQuota { get; set; } = "0";
        public bool TransactionPrintReceipt { get; set; } = true;
        public Dictionary<string, bool> TransactionReceiptFields { get; set; } = new()
        {
            {"store_name", true },
            {"phone", true },
            {"email", false },
            {"address", true },
            {"cashier", true },
            {"note", false }
        };
        public Dictionary<string, string> TransactionReceiptData { get; set; } = new()
        {
            {"store_name", "WRCay Hardware" },
            {"phone","0923 324 4235" },
            {"email", string.Empty },
            {"address", "Taguig City" },
            {"cashier", string.Empty },
            {"note", string.Empty }
        };
    }
}
