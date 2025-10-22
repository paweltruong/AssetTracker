using AssetTracker.Core.Models;
using AssetTracker.Core.Services.AssetsImporter.Definitions;

namespace AssetTracker.Core.Services.AssetsImporter
{
    public interface IAssetsImporter
    {
        /// <summary>
        /// Url of the page from which assets are imported (for example first page if pagination is present)
        /// </summary>
        public string ImporterAssetsUrl { get; }
        Task<IEnumerable<OwnedAsset>> ImportAssetsAsync(CancellationToken cancellationToken = default);
        Task<WebScrapingResult> ImportAssetsFromHtmlSourceAsync(string url, int pageNumber, string htmlSource, CancellationToken cancellationToken = default);
    }
}
