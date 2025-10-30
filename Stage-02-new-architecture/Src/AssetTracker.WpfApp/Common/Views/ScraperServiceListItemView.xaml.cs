using System.Windows.Controls;

namespace AssetTracker.WpfApp.Common.Views
{
    /// <summary>
    /// Interaction logic for ScraperServiceView.xaml
    /// Base class for scraper service list item views
    /// </summary>
    public partial class ScraperServiceListItemView : UserControl, IScraperServiceListItemView
    {
        public ScraperServiceListItemView()
        {
            InitializeComponent();
        }
    }
}
