using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMoDABot.Tools
{
    public static class UserMessageConverter
    {
        public static List<double> ToNumericArray(string message)
        {
            message = message.Replace(";", "");
            message = message.Replace(',', '.');

            string[] valueStrings = message.Split(' ', '\n', '\t');

            List<double> values = new List<double>();
            for (int i = 0; i < valueStrings.Length; ++i)
            {
                string input = valueStrings[i];
                if (String.IsNullOrWhiteSpace(input)) continue;

                try
                {
                    values.Add(Double.Parse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture));
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
            return values;
        }
    }
}