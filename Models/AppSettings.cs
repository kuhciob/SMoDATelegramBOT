using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMoDABot.Models
{
    public static class AppSettings
    {
        public static string Url { get; set; } = "https://smadcalc.azurewebsites.net:443/{0}";
        public static string Url2 { get; set; } = "https://smadcalc.azurewebsites.net/{0}";
        public static string Name { get; set; } = "smoda_bot";
        public static string Key { get; set; } = "1049740066:AAHLmSZ7I11XfNiK4NFbs9mRRFbnHgIgGKo";
    }
}