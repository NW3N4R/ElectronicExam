using Microsoft.UI.Xaml.Data;

using System;

namespace Examiner.Converters
{
    public class EqualsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
            => value?.ToString() == parameter?.ToString();

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => (bool)value ? parameter?.ToString() : null;
    }
}
