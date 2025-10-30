using AssetTracker.AssetsImporter.Steam.Definitions;
using AssetTracker.AssetsImporter.Steam.Services;
using AssetTracker.Core.Models;
using AssetTracker.Core.Models.Enums;
using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.AssetsImporter.Definitions;
using System.Linq;

namespace AssetTracker.AssetsImporter.Steam
{
    public class SteamAssetsImporter : IAssetsImporter
    {
        const string SteamStoreUrl = "https://store.steampowered.com";

        private readonly ISteamService _steamService;
        public string ImporterAssetsUrl => string.Empty;

        public SteamAssetsImporter(ISteamService steamService)
        {
            _steamService = steamService;
        }

        public Task<IEnumerable<OwnedAsset>> ImportAssetsAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<WebScrapingResult> ImportAssetsFromHtmlSourceAsync(string url, int pageNumber, string htmlSource, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OwnedAsset>> ImportAssetsFromHttpClientAsync(string importApiCallMethod, string importApiUrl, Dictionary<string, string> parameterValues, CancellationToken token)
        {
            var steamGames = await _steamService.GetSteamGamesAsync(
                steamApiKey: parameterValues[SteamAssetsImporterPlugin.ApiCallParamSteamApiKey],
                steamId: parameterValues[SteamAssetsImporterPlugin.ApiCallParamSteamId],
                cancellationToken: token);

            Dictionary<int, SteamAppData> details = new Dictionary<int, SteamAppData>();
            //foreach (var game in steamGames)
            //{
            //    var detail = await _steamService.GetSteamGameDetailsAsync(game.AppId, token);
            //    await Task.Delay(50);//Interval not to spam steam API
            //    details.Add(game.AppId, detail);
            //}

            var defaultTags = AssetTags.GetDefaultTags(AssetType.Game);
            var finalTags = new HashSet<string>();
            finalTags.Add(defaultTags);


            return steamGames.Select(game => new OwnedAsset(
                    marketplaceUid: game.AppId.ToString(),
                    name: game.Name,
                    assetType: AssetType.Game,
                    tags: finalTags,
                    imageUrl: game.FullImageUrl,
                    assetUrl: $"https://store.steampowered.com/app/{game.AppId}/",
                    publishers: details.ContainsKey(game.AppId) ? details[game.AppId].Publishers.Select(pub => new Publisher()
                    {
                        Name = pub,
                        Url = "https://store.steampowered.com/publisher/" + pub.Replace(" ", "")

                    }).ToList() : new List<Publisher>(),
                    developers: details.ContainsKey(game.AppId) ? details[game.AppId].Publishers.Select(dev => new Developer()
                    {
                        Name = dev,
                        Url = "https://store.steampowered.com/developer/" + dev.Replace(" ", "_")

                    }).ToList() : new List<Developer>(),
                    sourcePluginKey: SteamAssetsImporterPlugin.PluginKeyConst,
                    marketplaceKey: SteamAssetsImporterPlugin.PluginMarketplaceKeyConst,
                    marketplaceName: SteamAssetsImporterPlugin.PluginMarketplaceKeyConst,
                    marketplaceAccountId: parameterValues[SteamAssetsImporterPlugin.ApiCallParamSteamId],
                    marketplaceUrl: SteamStoreUrl,
                    searchKeywords: GetKeywordsFromName(game.Name))
            );
        }

        HashSet<string> GetKeywordsFromName(string name)
        {
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
    }
}
