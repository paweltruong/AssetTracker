using AssetTracker.AssetsImporter.Steam.Services;
using AssetTracker.Core.Models;
using AssetTracker.Core.Models.Enums;
using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.AssetsImporter.Definitions;

namespace AssetTracker.AssetsImporter.Steam
{
    public class SteamAssetsImporter : IAssetsImporter
    {
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

            return steamGames.Select(p => new OwnedAsset(
                    marketplaceUid: p.AppId.ToString(),
                    name: p.Name,
                    assetType: AssetType.Game,
                    imageUrl: p.FullImageUrl,
                    assetUrl: $"https://store.steampowered.com/app/{p.AppId}/",
                    sourcePluginKey: SteamAssetsImporterPlugin.PluginKeyConst,
                    SteamAssetsImporterPlugin.PluginMarketplaceKeyConst)
            );
        }
    }
}
