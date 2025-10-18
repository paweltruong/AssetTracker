using System.Globalization;
using System.Windows.Data;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Converters
{
    public class SteamImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imageUrl && !string.IsNullOrEmpty(imageUrl))
            {
                // Steam returns relative URLs, construct full URL
                if (!imageUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    // Get the appId from the data context if possible
                    if (parameter is int appId)
                    {
                        return $"https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/{appId}/{imageUrl}.jpg";
                    }

                    // Fallback - you might need to adjust this based on your data structure
                    return $"https://cdn.cloudflare.steamstatic.com/steamcommunity/public/images/apps/unknown/{imageUrl}.jpg";
                }
                return imageUrl;
            }

            // Return a default game icon or null
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
