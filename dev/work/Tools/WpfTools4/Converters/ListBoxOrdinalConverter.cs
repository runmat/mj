using System.Windows.Controls;
using System.Windows.Data;

namespace WpfTools4.Converters
{
    public class ListBoxOrdinalConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var lvi = value as ListBoxItem;
            var ordinal = 0;

            if (lvi != null)
            {
                var lv = ItemsControl.ItemsControlFromItemContainer(lvi) as ListBox;
                if (lv != null)
                    ordinal = lv.ItemContainerGenerator.IndexFromContainer(lvi) + 1;
            }

            return string.Format("{0}.", ordinal);

        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // This converter does not provide conversion back from ordinal position to list view item
            throw new System.InvalidOperationException();
        }
    }
}
