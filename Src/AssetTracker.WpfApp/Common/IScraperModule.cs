using AssetTracker.WpfApp.Common.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.WpfApp.Common
{
    /// <summary>
    /// Interface for scraper services
    /// </summary>
    public interface IScraperModule
    {
        void ConfigureModule(IServiceCollection services);
        IScraperServiceView GetView(IServiceProvider serviceProvider);
    }
}
