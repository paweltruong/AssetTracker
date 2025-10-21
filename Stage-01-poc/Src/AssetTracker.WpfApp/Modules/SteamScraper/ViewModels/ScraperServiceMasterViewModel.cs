using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.SteamScraper.Views;

namespace AssetTracker.WpfApp.Modules.SteamScraper.ViewModels
{
    public class ScraperServiceMasterViewModel : IScraperServiceMasterModel
    {
        public string ModuleName => SteamScraperModule.ModuleName;

        public ScraperServiceMasterViewModel(ScraperListItemView listItemView, ScraperMainView mainView, ScrapeWizardView importAssetsView)
        {
            ListItemView = listItemView;
            DefaultMainView = mainView;
            ImportAssetsView = importAssetsView;

        }
        public IScraperServiceListItemView? ListItemView { get; private set; }

        public IScraperServiceMainView? DefaultMainView { get; private set; }

        public IScraperServiceMainView? ImportAssetsView { get; private set; }
    }
}
