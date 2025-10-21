using AssetTracker.WpfApp.Modules.SteamScraper.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace AssetTracker.WpfApp.Common.Views
{
    /// <summary>
    /// Interaction logic for DefaultBrowserAssetsImporterView.xaml
    /// </summary>
    public partial class DefaultBrowserAssetsImporterView : UserControl, IScraperServiceMainView
    {
        public DefaultBrowserAssetsImporterView()
        {
            InitializeComponent();
        }
        private void SteamIdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ScrapeWizardViewModel;
            if (viewModel != null)
            {
                viewModel.SteamId = ((PasswordBox)sender).Password;
            }
        }

        private void SteamApiBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as ScrapeWizardViewModel;
            if (viewModel != null)
            {
                viewModel.SteamApiKey = ((PasswordBox)sender).Password;
            }
        }
    }
}
