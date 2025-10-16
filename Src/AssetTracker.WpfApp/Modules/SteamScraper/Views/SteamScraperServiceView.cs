using AssetTracker.WpfApp.Common.Views;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Views
{
    public class SteamScraperServiceView : IScraperServiceView
    {
        SteamScraperListItemView _listItemView;
        SteamScraperMainView _mainView;

        public SteamScraperServiceView(SteamScraperListItemView listItemView, SteamScraperMainView mainView)
        {
            _listItemView = listItemView;
            _mainView = mainView;
        }
        public IScraperServiceListItemView? ListItemView => _listItemView;

        public IScraperServiceMainView? MainView => _mainView;
    }
}
