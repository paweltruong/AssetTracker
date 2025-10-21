using AssetTracker.Core.Models;
using AssetTracker.Core.Services.AssetsImporter.Definitions;

namespace AssetTracker.Core.Services.AssetsImporter
{
    public interface IAssetsImporter
    {
        Task<IEnumerable<OwnedAsset>> ImportAssetsAsync(CancellationToken cancellationToken = default);

        Task<WebScrapingResult> ImportAssetsFromHtmlSourceAsync(string url, string htmlSource, CancellationToken cancellationToken = default);
    }
}
