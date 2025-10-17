using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.SteamScraper.Views;

namespace AssetTracker.WpfApp.Modules.SteamScraper.ViewModels
{
    public class ScraperServiceMasterViewModel : IScraperServiceMasterViewModel
    {
        ScraperListItemView _listItemView;
        ScraperMainView _mainView;

        public ScraperServiceMasterViewModel(ScraperListItemView listItemView, ScraperMainView mainView)
        {
            _listItemView = listItemView;
            _mainView = mainView;
        }
        public IScraperServiceListItemView? ListItemView => _listItemView;

        public IScraperServiceMainView? DefaultMainView => _mainView;
    }
}
