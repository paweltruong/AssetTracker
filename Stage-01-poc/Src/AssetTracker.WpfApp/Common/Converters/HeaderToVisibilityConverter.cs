using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AssetTracker.WpfApp.Common.Converters
{
    public class HeaderToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string currentSortedColumn = value as string;
            string thisColumnHeader = parameter as string;

            return currentSortedColumn == thisColumnHeader ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
