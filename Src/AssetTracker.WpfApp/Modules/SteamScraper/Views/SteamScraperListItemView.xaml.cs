using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.SteamScraper.ViewModels;
using System.Windows.Controls;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Views
{
    /// <summary>
    /// Interaction logic for SteamScraperListItemView.xaml
    /// </summary>
    public partial class SteamScraperListItemView : UserControl, IScraperServiceListItemView
    {
        public SteamScraperListItemView(SteamScraperServiceViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
