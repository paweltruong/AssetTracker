using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Common.Views;

using AssetTracker.WpfApp.Modules.SteamScraper;

namespace AssetTracker.WpfApp.Modules.HumbleBundle.Scrapper.ViewModels
{
    public class HumbleBundleServiceMasterViewModel : IScraperServiceMasterModel
    {
        IScraperServiceListItemView _listItemView;
        IScraperServiceMainView _mainView;

        public string ModuleName => HumbleBundleModule.ModuleName;

        //public HumbleBundleServiceMasterViewModel(ScraperListItemView listItemView, ScraperMainView mainView)
        //{
        //    _listItemView = listItemView;
        //    _mainView = mainView;
        //}
        public IScraperServiceListItemView? ListItemView => _listItemView;

        public IScraperServiceMainView? DefaultMainView => _mainView;

    }
}
