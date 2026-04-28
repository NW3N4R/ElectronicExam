using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElectronicExam.Administrator.valueConverters
{
    public class GenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return "";

            var gender = value.ToString();

            return gender switch
            {
                "M" => "Male",
                "F" => "Female",
                _ => "Unknown"
            };
        }

        // Convert back string -> char (Male -> M)
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return 'U';

            var text = value.ToString()?.ToLower();

            return text switch
            {
                "male" => 'M',
                "female" => 'F',
                _ => 'U'
            };
        }
    }
}
