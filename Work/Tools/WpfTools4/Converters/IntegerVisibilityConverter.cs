using System;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace WpfTools4.Converters
{
    [ValueConversion(typeof(Visibility), typeof(int))]
    public class IntegerVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            dynamic dValue = 0;
            if (value is long)
                dValue = (long)value;
            if (value is int)
                dValue = (int)value;

            if (parameter == null)
                return (dValue != 0 && dValue != -1 ? Visibility.Visible : Visibility.Collapsed);

            return ((int)dValue == Int32.Parse(parameter.ToString()) ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

    [ValueConversion(typeof(Visibility), typeof(int))]
    public class IntegerVisibilityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            dynamic dValue = 0;
            if (value is long)
                dValue = (long)value;
            if (value is int)
                dValue = (int)value;

            if (parameter == null)
                return (dValue != 0 && dValue != -1 ? Visibility.Collapsed : Visibility.Visible);

            return ((int)dValue == Int32.Parse(parameter.ToString()) ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
