using Microsoft.UI.Xaml.Data;

using System;

namespace ElectronicExam.Administrator.valueConverters
{
    public class StringToDecimal : IValueConverter
    {
        // From Database (decimal) to UI (string)
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is decimal hours)
            {
                return hours.ToString("F1"); // one decimal place
            }
            return "1.0";
        }

        // From UI (string) to Database (decimal)
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string str && decimal.TryParse(str, out decimal result))
            {
                return result;
            }
            return 1.0m; // use decimal literal
        }
    }
}
