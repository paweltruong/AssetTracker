using System.Globalization;
using System.Windows.Data;

namespace AssetTracker.WpfApp.Common.Converters
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double percentage)
            {
                return $"{percentage:P0}"; // or use P1, P2 for decimal places
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
