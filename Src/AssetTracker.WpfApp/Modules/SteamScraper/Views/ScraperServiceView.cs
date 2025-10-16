using AssetTracker.WpfApp.Common.Views;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Views
{
    public class ScraperServiceView : IScraperServiceView
    {
        ScraperListItemView _listItemView;
        ScraperMainView _mainView;

        public ScraperServiceView(ScraperListItemView listItemView, ScraperMainView mainView)
        {
            _listItemView = listItemView;
            _mainView = mainView;
        }
        public IScraperServiceListItemView? ListItemView => _listItemView;

        public IScraperServiceMainView? MainView => _mainView;
    }
}
