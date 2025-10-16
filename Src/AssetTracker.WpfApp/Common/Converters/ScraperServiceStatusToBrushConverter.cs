using AssetTracker.WpfApp.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace AssetTracker.WpfApp.Common.Converters
{
    public class ScraperServiceStatusToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ScraperServiceStatus status)
            {
                // Define colors for each status
                return status switch
                {
                    ScraperServiceStatus.Unknown => new SolidColorBrush(Colors.Red),
                    ScraperServiceStatus.NotLoaded => new SolidColorBrush(Colors.Gray),
                    ScraperServiceStatus.Failed => new SolidColorBrush(Colors.Red),
                    ScraperServiceStatus.Running => new SolidColorBrush(Colors.Blue),
                    ScraperServiceStatus.DataLoaded => new SolidColorBrush(Colors.Green),
                    _ => new SolidColorBrush(Colors.Black)
                };
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
