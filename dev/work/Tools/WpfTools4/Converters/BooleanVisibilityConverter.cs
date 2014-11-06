using System;
using System.Windows.Data;
using System.Windows;
using System.Globalization;

namespace WpfTools4.Converters
{
    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((Visibility)value) == Visibility.Visible);
        }
    }

    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class BooleanVisibilityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (!(bool)value ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((Visibility)value) != Visibility.Visible);
        }
    }

    //
    // Preserver Space ==> for "false" <==> no "Collapsed" but "Hidden"
    //

    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class BooleanVisibilityPreserveSpaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value ? Visibility.Visible : Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((Visibility)value) == Visibility.Visible);
        }
    }

    [ValueConversion(typeof(Visibility), typeof(bool))]
    public class BooleanVisibilityPreserveSpaceInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (!(bool)value ? Visibility.Visible : Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((Visibility)value) != Visibility.Visible);
        }
    }
}
