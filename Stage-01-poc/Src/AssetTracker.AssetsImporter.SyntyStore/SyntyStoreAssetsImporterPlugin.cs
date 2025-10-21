using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.AssetsImporter.SyntyStore
{
    public class SyntyStoreAssetsImporterPlugin : IAssetsImporterPlugin
    {
        public const string PluginKeyConst = "SyntyStoreAssetsImporterPlugin";
        public string PluginKey => PluginKeyConst;

        public bool UseDefaultBrowserLayout => true;

        public string DisplayName => "SyntyStore";

        public string Description => "Import products from SyntyStore";

        public string IconUrl => "https://syntystore.com/cdn/shop/files/Black_Synty_Store_logo_4x_f2782732-f238-4f7c-b1e5-b07e8ef286d5.png?v=1676510171&width=200";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddKeyedSingleton<IAssetsImporter, SyntyStoreAssetsImporter>(PluginKey);
        }
    }
}
