using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AssetTracker.WpfApp.Common.Converters
{
    public class ColorToBrushConverter : IValueConverter
    {
        public static ColorToBrushConverter Instance { get; } = new ColorToBrushConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorString)
            {
                try
                {
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorString));
                }
                catch
                {
                    return new SolidColorBrush(Colors.Gray);
                }
            }
            if(value is Color color)
            {
                return new SolidColorBrush(color);
            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
