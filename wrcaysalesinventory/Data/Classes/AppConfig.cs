using HandyControl.Themes;
using System;

namespace wrcaysalesinventory.Data.Classes
{
    public class AppConfig
    {
        public static readonly string SavePath = $"{AppDomain.CurrentDomain.BaseDirectory}AppConfig.json";

        public string Lang { get; set; } = "en-US";

        public ApplicationTheme Theme { get; set; }
    }
}
