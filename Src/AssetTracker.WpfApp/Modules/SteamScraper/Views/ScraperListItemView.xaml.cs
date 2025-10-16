using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.SteamScraper.ViewModels;
using System.Windows.Controls;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Views
{
    /// <summary>
    /// Interaction logic for SteamScraperListItemView.xaml
    /// </summary>
    public partial class ScraperListItemView : UserControl, IScraperServiceListItemView
    {
        public ScraperListItemView(ScraperListItemViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
