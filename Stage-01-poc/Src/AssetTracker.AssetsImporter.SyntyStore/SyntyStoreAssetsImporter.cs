using AssetTracker.Application.Services;
using AssetTracker.AssetsImporter.SyntyStore.Services;
using AssetTracker.Core.Models;
using AssetTracker.Core.Models.Enums;
using AssetTracker.Core.Services.AssetsImporter;

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

        public async Task<IEnumerable<OwnedAsset>> ImportAssetsAsync(CancellationToken cancellationToken = default)
        {
            var results = await _syntyStoreScraper.ScrapeAllProductsAsync();

            var assets = results.Products.Where(p => !string.IsNullOrEmpty(p.Title))
                .Select(p => new OwnedAsset
                {
                    Name = p.Title,
                    AssetType = AssetType.Software,
                    //TODO default tags
                    //public HashSet<string>? Tags { get; set; }
                    ImageUrl = p.ThumbnailUrl,
                    Publishers = new List<Publisher>(),
                    Developers = new List<Developer>()
                    {
                        new Developer { Name = "Synty Studios" }
                    },
                    AssetUrl = p.FullDownloadUrl,
                    MarketplaceName = "Synty Store",
                    MarketplaceAccountId = "paweltruong@o2.pl",
                    MarketplaceUrl = "https://syntystore.com/"
                });
            return assets;
        }
    }
}
