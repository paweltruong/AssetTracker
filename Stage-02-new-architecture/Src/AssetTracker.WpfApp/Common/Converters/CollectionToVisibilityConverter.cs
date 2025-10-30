using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AssetTracker.WpfApp.Common.Converters
{
    public class CollectionToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is System.Collections.IEnumerable enumerable)
            {
                // Check if it's a collection of IWpfModel and has items
                var enumerator = enumerable.GetEnumerator();
                bool hasItems = enumerator.MoveNext();

                // Reset the enumerator if it's IDisposable
                if (enumerator is IDisposable disposable)
                    disposable.Dispose();

                return hasItems ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("CollectionToVisibilityConverter is a one-way converter");
        }
    }
}
