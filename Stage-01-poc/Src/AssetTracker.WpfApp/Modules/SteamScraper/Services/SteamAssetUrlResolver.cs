using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Services.AssetsResolver;
using AssetTracker.WpfApp.Common.Services.AssetsResolver.Definitions;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Services
{
    public class SteamAssetUrlResolver : IAssetsResolver
    {
        public async Task<AssetsResolverResult> ResolveAssetFromUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            await Task.Delay(5000, cancellationToken); // Simulate some async work

            return new AssetsResolverResult
            {
                WasSuccessful = true,
                Items = new List<AssetItem>()
            };
        }
    }
}
