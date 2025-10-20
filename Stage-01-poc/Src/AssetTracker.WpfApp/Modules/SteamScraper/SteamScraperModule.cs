using AssetTracker.Core.Services.AssetsResolver;
using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Modules.SteamScraper.Services;
using AssetTracker.WpfApp.Modules.SteamScraper.ViewModels;
using AssetTracker.WpfApp.Modules.SteamScraper.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.WpfApp.Modules.SteamScraper
{
    public class SteamScraperModule : IScraperModule
    {
        public const string ModuleName = "SteamScraper";
        public void ConfigureModule(IServiceCollection services)
        {
            services.AddKeyedSingleton<IAssetsResolver, SteamAssetsResolver>(ModuleName);
            services.AddSingleton<ScraperServiceMasterViewModel>();
            services.AddSingleton<ScraperListItemViewModel>();
            services.AddSingleton<ScraperListItemView>();
            services.AddSingleton<ScraperMainView>();
            services.AddSingleton<ScrapeWizardViewModel>();
            services.AddSingleton<ScrapeWizardView>();
            services.AddSingleton<DataView>();
            services.AddSingleton<ISteamService, SteamService>();
        }

        public IScraperServiceMasterModel GetMasterModel(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<ScraperServiceMasterViewModel>();
        }
    }
}
