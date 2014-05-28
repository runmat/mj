using System;
using System.Windows.Data;
using System.Globalization;

namespace WpfTools4.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class ListboxItemStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((string)value).Replace("_", "-");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new NotSupportedException();
        }
    }
}
