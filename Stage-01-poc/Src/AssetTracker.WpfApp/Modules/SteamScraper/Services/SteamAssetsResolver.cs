using AssetTracker.Core.Models;
using AssetTracker.Core.Services.AssetsResolver;
using AssetTracker.Core.Services.AssetsResolver.Definitions;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Services
{
    public class SteamAssetsResolver : IAssetsResolver
    {
        public const string SteamStoreLinkPart = "https://store.steampowered.com/app/";
        public async Task<AssetsResolverResult> ResolveAssetFromUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            if(!url.StartsWith(SteamStoreLinkPart))
            {
                return new AssetsResolverResult
                {
                    WasSuccessful = false,
                    Items = new List<Asset>()
                };
            }

            await Task.Delay(5000, cancellationToken); // Simulate some async work

            return new AssetsResolverResult
            {
                WasSuccessful = true,
                Items = new List<Asset>()
            };
        }
    }
}
