using AssetTracker.WpfApp.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AssetTracker.WpfApp.Common.Converters
{
    public class ScraperServiceStatusToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ScraperServiceStatus status)
            {
                // Customize this based on how you want to display each enum value
                return status switch
                {
                    _ => status.ToString()
                };
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
