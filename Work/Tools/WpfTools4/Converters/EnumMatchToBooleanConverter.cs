using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace WpfTools4.Converters
{
    [ValueConversion(typeof(Enum), typeof(bool))]
    public class EnumMatchToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return (value.ToString() == parameter.ToString());
            }
            catch
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Enum.Parse(targetType, parameter.ToString());
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
