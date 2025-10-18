namespace AssetTracker.WpfApp.Common.Utils
{
    public static class HttpHelpers
    {
        public static bool IsValidHttpUrl(string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult))
            {
                return uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
            }
            return false;
        }
    }
}
