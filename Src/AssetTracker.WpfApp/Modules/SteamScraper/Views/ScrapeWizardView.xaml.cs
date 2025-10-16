using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.SteamScraper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
