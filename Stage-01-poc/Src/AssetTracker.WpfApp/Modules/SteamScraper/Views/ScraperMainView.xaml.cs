using AssetTracker.WpfApp.Common.Views;
using System.Windows.Controls;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Views
{
    /// <summary>
    /// Interaction logic for SteamScraperMainView.xaml
    /// </summary>
    public partial class ScraperMainView : UserControl, IScraperServiceMainView
    {
        public ScraperMainView()
        {
            InitializeComponent();
        }
    }
}
