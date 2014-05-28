using System;
using System.Windows.Data;
using System.Globalization;

namespace WpfTools4.Converters
{
    [ValueConversion(typeof(int), typeof(bool))]
    public class BooleanToIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return ((bool)value ? 1 : 0);
            }
            catch
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return false;
            
            if (value.ToString() == "")
                return false;

            int val;
            if (!Int32.TryParse(value.ToString(), out val))
                return false;

            return (val != 0);
        }
    }
}
