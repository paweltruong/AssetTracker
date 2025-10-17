using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.SteamScraper.ViewModels;
using System.Windows.Controls;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Views
{
    /// <summary>
    /// Interaction logic for DataView.xaml
    /// </summary>
    public partial class DataView : UserControl, IScraperServiceMainView
    {
        public DataView(ScrapeWizardViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
