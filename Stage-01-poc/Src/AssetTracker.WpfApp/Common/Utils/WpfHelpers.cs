using System.Diagnostics;
using System.Windows;

namespace AssetTracker.WpfApp.Common.Utils
{
    public static class WpfHelpers
    {
        public static void OpenUrl(string url)
        {
            if (!string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            else
            {
                MessageBox.Show($"Invalid url {url}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
