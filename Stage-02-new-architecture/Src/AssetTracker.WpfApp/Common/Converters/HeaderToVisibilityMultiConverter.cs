using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AssetTracker.WpfApp.Common.Converters
{
    public class HeaderToVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return Visibility.Collapsed;

            string? sortedColumn = values[0]?.ToString();
            string? columnTag = values[1]?.ToString();

            if (string.IsNullOrWhiteSpace(sortedColumn) || string.IsNullOrWhiteSpace(columnTag))
                return Visibility.Collapsed;

            return string.Equals(sortedColumn.Trim(), columnTag.Trim(), StringComparison.OrdinalIgnoreCase)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
