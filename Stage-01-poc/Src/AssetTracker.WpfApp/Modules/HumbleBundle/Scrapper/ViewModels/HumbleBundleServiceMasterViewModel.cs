using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Common.Views;

using AssetTracker.WpfApp.Modules.SteamScraper;

namespace AssetTracker.WpfApp.Modules.HumbleBundle.Scrapper.ViewModels
{
    public class HumbleBundleServiceMasterViewModel : IScraperServiceMasterModel
    {
        public string ModuleName => HumbleBundleModule.ModuleName;

        //public HumbleBundleServiceMasterViewModel(ScraperListItemView listItemView, ScraperMainView mainView)
        //{
        //    _listItemView = listItemView;
        //    _mainView = mainView;
        //}
        public IScraperServiceListItemView? ListItemView { get; private set; }

        public IScraperServiceMainView? DefaultMainView { get; private set; }

        public IScraperServiceMainView? ImportAssetsView { get; private set; }
    }
}
