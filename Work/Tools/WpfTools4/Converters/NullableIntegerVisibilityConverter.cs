using System;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace WpfTools4.Converters
{
    [ValueConversion(typeof(Visibility), typeof(int?))]
    public class NullableIntegerVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((int?)value).HasValue ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
