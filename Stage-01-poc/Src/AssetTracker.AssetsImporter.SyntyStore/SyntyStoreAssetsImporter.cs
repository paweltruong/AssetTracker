using AssetTracker.Application.Services;
using AssetTracker.AssetsImporter.SyntyStore.Services;
using AssetTracker.Core.Models;
using AssetTracker.Core.Models.Enums;
using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.AssetsImporter.Definitions;
using System.Linq;

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

            var defaultTags = AssetTags.GetDefaultTags(AssetType.Software);
            var finalTags = new HashSet<string>();
            finalTags.Add(defaultTags);

            return new WebScrapingResult
            {
                SourceUrl = url,
                PageNumber = pageNumber,
                Success = result.Success,
                ErrorMessage = result.ErrorMessage,
                OwnedAssets = result.Products.Select(asset => new OwnedAsset(
                    marketplaceUid: asset.Title?.ToLowerInvariant()?.Replace(" ", "-")?.Trim(),
                    name: asset.Title,
                    assetType: AssetType.Software,
                    tags: finalTags,
                    imageUrl: asset.ThumbnailUrl,
                    assetUrl: asset.FullDownloadUrl,
                    publishers: new List<Publisher>(),
                    developers: new List<Developer>() { new Developer
                        {
                            Name = "Synty Studios",
                            Url = "https://syntystore.com/"
                        }
                    },
                    sourcePluginKey: SyntyStoreAssetsImporterPlugin.PluginKeyConst,
                    marketplaceKey: SyntyStoreAssetsImporterPlugin.PluginMarketplaceKeyConst,
                    marketplaceName: SyntyStoreAssetsImporterPlugin.PluginMarketplaceKeyConst,
                    marketplaceAccountId: string.Empty,
                    marketplaceUrl: "https://syntystore.com/",
                    searchKeywords: GetKeywordsFromName(asset.Title))
                ),
                NextPageUrl = result.NextPageUrl
            };
        }

        string GetNameForSearch(string nameFromDownloads)
        {
            return nameFromDownloads.Replace("Pack", "", StringComparison.OrdinalIgnoreCase)
                .Replace("-", " ")
                .Replace("(Unity only)", "", StringComparison.OrdinalIgnoreCase);
        }


        HashSet<string> GetKeywordsFromName(string name)
        {
            string nameFromDownloads = name.Replace("Pack", "", StringComparison.OrdinalIgnoreCase)
                .Replace("-", " ")
                .Replace("(Unity only)", "", StringComparison.OrdinalIgnoreCase);

            HashSet<string> keywords = new HashSet<string>();
            var items = name.Split(new char[] { ' ', '-', '_', ':' });
            foreach (var item in items)
            {
                var trimmedItem = item.Trim().ToLower();
                if (!string.IsNullOrEmpty(trimmedItem))
                {
                    keywords.Add(trimmedItem);
                }
            }
            return keywords;
        }

        public Task<IEnumerable<OwnedAsset>> ImportAssetsFromHttpClientAsync(string importApiCallMethod, string importApiUrl, Dictionary<string, string> parameterValues, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
