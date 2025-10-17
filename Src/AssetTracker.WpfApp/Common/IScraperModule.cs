using AssetTracker.WpfApp.Common.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.WpfApp.Common
{
    /// <summary>
    /// Interface for scraper services
    /// </summary>
    public interface IScraperModule
    {
        void ConfigureModule(IServiceCollection services);
        IScraperServiceMasterViewModel GetMasterViewModel(IServiceProvider serviceProvider);
    }
}
