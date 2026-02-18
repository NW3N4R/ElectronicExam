using Microsoft.UI.Xaml.Data;

using System;

namespace ElectronicExam.Administrator.valueConverters
{
    internal class StringToByteConverter : IValueConverter
    {
        // Source -> UI (byte to string)
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value?.ToString() ?? "0";
        }

        // UI -> Source (string to byte)
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string input = value as string;

            if (byte.TryParse(input, out byte result))
            {
                return result;
            }

            // Return 0 (or a default) if parsing fails
            return (byte)0;
        }
    }
}
