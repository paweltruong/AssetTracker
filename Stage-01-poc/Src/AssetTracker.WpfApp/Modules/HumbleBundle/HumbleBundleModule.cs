using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Modules.HumbleBundle.Scrapper.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.WpfApp.Modules.SteamScraper
{
    public class HumbleBundleModule : IScraperModule
    {
        public const string ModuleName = "HumbleBundleScraper";
        public void ConfigureModule(IServiceCollection services)
        {
            services.AddSingleton<HumbleBundleServiceMasterViewModel>();
        }

        public IScraperServiceMasterModel GetMasterModel(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<HumbleBundleServiceMasterViewModel>();
        }
    }
}
