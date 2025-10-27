using System.Diagnostics;
using System.IO;
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

        public static bool CanOpenPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            if (!Path.Exists(path)) return false;

            return true;
        }

        public static void OpenPath(string path)
        {

            try
            {
                if (System.IO.File.Exists(path))
                {
                    Process.Start("explorer.exe", $"/select,\"{path}\"");
                }
                else if (System.IO.Directory.Exists(path))
                {
                    Process.Start("explorer.exe", $"\"{path}\"");
                }
                else
                {
                    // Try to open the parent directory if the file doesn't exist
                    string directory = System.IO.Path.GetDirectoryName(path);
                    if (System.IO.Directory.Exists(directory))
                    {
                        Process.Start("explorer.exe", $"\"{directory}\"");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error or show message
                System.Diagnostics.Debug.WriteLine($"Error opening file location: {ex.Message}");
            }
        }
    }
}
