using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.Core.Services.Plugins
{
    public interface IPlugin
    {
        public string PluginKey { get; }
        public string MarketplaceKey { get; }

        public void ConfigureServices(IServiceCollection services);
    }
}
