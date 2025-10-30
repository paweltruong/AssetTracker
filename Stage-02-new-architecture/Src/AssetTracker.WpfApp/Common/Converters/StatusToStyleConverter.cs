using AssetTracker.Core.Models.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace AssetTracker.WpfApp.Common.Converters
{
    public class StatusToStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AssetComparisonStatus status)
            {
                var style = new Style(typeof(TextBlock));

                style.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));

                switch (status)
                {
                    case AssetComparisonStatus.NotExists:
                        style.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Green));
                        break;
                    case AssetComparisonStatus.Processing:
                    case AssetComparisonStatus.Exists:
                        style.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.DarkOrange));
                        break;
                    case AssetComparisonStatus.Error:
                        style.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Red));
                        break;
                    default:
                        style.Setters.Add(new Setter(TextBlock.ForegroundProperty, Brushes.Gray));
                        break;
                }

                return style;
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
