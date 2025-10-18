using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.SteamScraper.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Views
{
    /// <summary>
    /// Interaction logic for ScrapeWizardView.xaml
    /// </summary>
    public partial class ScrapeWizardView : UserControl, IScraperServiceMainView
    {
        public ScrapeWizardView(ScrapeWizardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
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
