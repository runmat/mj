using System;
using System.Windows.Data;
using System.Globalization;

namespace WpfTools4.Converters
{
    [ValueConversion(typeof(int), typeof(string))]
    public class IntegerToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) 
                return "";

            return ((int)value).ToString("#");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ToInt32((string)value);
            }
            catch
            {
                return 0;
            }
        }
    }
}
