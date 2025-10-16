using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.SteamScraper.ViewModels;
using AssetTracker.WpfApp.Modules.SteamScraper.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.WpfApp.Modules.SteamScraper
{
    public class SteamScraperModule : IScraperModule
    {
        public void ConfigureModule(IServiceCollection services)
        {
            services.AddSingleton<SteamScraperServiceViewModel>();
            services.AddSingleton<SteamScraperServiceView>();
            services.AddSingleton<SteamScraperListItemView>();
            services.AddSingleton<SteamScraperMainView>();
        }

        public IScraperServiceView GetView(IServiceProvider serviceProvider)
        {
            return serviceProvider.GetRequiredService<SteamScraperServiceView>();
        }
    }
}
