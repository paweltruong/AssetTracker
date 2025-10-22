using AssetTracker.Application.Services;
using AssetTracker.AssetsImporter.SyntyStore.Services;
using AssetTracker.Core.Models;
using AssetTracker.Core.Models.Enums;
using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.AssetsImporter.Definitions;

namespace AssetTracker.AssetsImporter.SyntyStore
{
    public class SyntyStoreAssetsImporter : IAssetsImporter
    {
        IMyHttpClient _httpClient;
        SyntyStoreScraper _syntyStoreScraper;

        public SyntyStoreAssetsImporter(IMyHttpClient httpClient)
        {
            _httpClient = httpClient;
            _syntyStoreScraper = new SyntyStoreScraper();
        }

        public string ImporterAssetsUrl => "https://syntystore.com/apps/downloads/orders";

        public async Task<IEnumerable<OwnedAsset>> ImportAssetsAsync(CancellationToken cancellationToken = default)
        {
            //var results = await _syntyStoreScraper.ScrapeAllProductsAsync();

            //var assets = results.Products.Where(p => !string.IsNullOrEmpty(p.Title))
            //    .Select(p => new OwnedAsset
            //    {
            //        Name = p.Title,
            //        AssetType = AssetType.Software,
            //        //TODO default tags
            //        //public HashSet<string>? Tags { get; set; }
            //        ImageUrl = p.ThumbnailUrl,
            //        Publishers = new List<Publisher>(),
            //        Developers = new List<Developer>()
            //        {
            //            new Developer { Name = "Synty Studios" }
            //        },
            //        AssetUrl = p.FullDownloadUrl,
            //        MarketplaceName = "Synty Store",
            //        MarketplaceAccountId = "paweltruong@o2.pl",
            //        MarketplaceUrl = "https://syntystore.com/"
            //    });
            //return assets;
            return Enumerable.Empty<OwnedAsset>();
        }

        public async Task<WebScrapingResult> ImportAssetsFromHtmlSourceAsync(string url, int pageNumber, string htmlSource, CancellationToken cancellationToken = default)
        {
            var result = await _syntyStoreScraper.ScrapeAllProductsAsync(url, htmlSource, cancellationToken);
            return new WebScrapingResult
            {
                SourceUrl = url,
                PageNumber = pageNumber,
                Success = result.Success,
                ErrorMessage = result.ErrorMessage,
                OwnedAssets = result.Products.Select(p => new OwnedAsset
                {
                    Name = p.Title,
                    AssetType = AssetType.Software,
                    ImageUrl = p.ThumbnailUrl,
                    AssetUrl = p.FullDownloadUrl,
                }),
                NextPageUrl = result.NextPageUrl
            };
        }
    }
}
