using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicExam.Administrator.valueConverters
{
    public class DateTimeToStringConverter : IValueConverter
    {
        // DateTime -> string
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime dt )
            {
                return dt.ToString("yyyy-MM-dd hh:mm tt");
            }
            if(value is DateTimeOffset dto)
            {
                return dto.ToString("yyyy-MM-dd hh:mm tt");
            }

            return "";
        }

        // string -> DateTime (optional, usually not needed)
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (DateTime.TryParse(value?.ToString(), out var dt))
            {
                return dt;
            }

            return DateTime.MinValue;
        }
    }
}
