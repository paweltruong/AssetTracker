using AssetTracker.WpfApp.Common;
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
            services.AddSingleton<ScraperListItemViewModel>();
            services.AddSingleton<ScraperServiceView>();
            services.AddSingleton<ScraperListItemView>();
            services.AddSingleton<ScraperMainView>();
            services.AddSingleton<ScrapeWizardViewModel>();
            services.AddSingleton<ScrapeWizardView>();
            services.AddSingleton<ISteamService, SteamService>();
        }

        public AssetTracker.WpfApp.Common.Views.IScraperServiceView GetView(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<ScraperServiceView>();
        }
    }
}
