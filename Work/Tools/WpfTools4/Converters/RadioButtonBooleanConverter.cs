using System;
using System.Windows.Data;
using System.Globalization;

namespace WpfTools4.Converters
{

    [ValueConversion(typeof(int), typeof(bool))]
    public class RadioButtonBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return (Int64.Parse(parameter.ToString()) == -1);

            int integer = (int)value;
            if (integer == Int64.Parse(parameter.ToString()))
                return true;
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.Parse(value.ToString()) == true)
                return parameter;
            else
                return null;
        }
    }
}
