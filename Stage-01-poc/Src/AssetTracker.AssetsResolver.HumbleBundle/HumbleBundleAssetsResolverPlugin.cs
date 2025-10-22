using AssetTracker.Core.Services.AssetsResolver;
using AssetTracker.Core.Services.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.AssetsResolver.HumbleBundle
{
    public class HumbleBundleAssetsResolverPlugin : IAssetsResolverPlugin
    {
        public string PluginKey => "HumbleBundleAssetsResolver";
        public string MarketplaceKey => "HumbleBundle";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddKeyedSingleton<IAssetsResolver, HumbleBundleAssetsResolver>(PluginKey);
        }
    }
}
