using AssetTracker.AssetsImporter.Steam.Services;
using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.AssetsImporter.Steam
{
    public class SteamAssetsImporterPlugin : IAssetsImporterPlugin
    {
        public const string PluginKeyConst = "SteamAssetsImporterPlugin";

        public const string PluginMarketplaceKeyConst = "Steam";

        public const string ApiCallParamSteamApiKey = "steamApiKey";
        public const string ApiCallParamSteamId = "steamId";
        public const string ApiCallParamSteamApiKeyTutorial = @"
If you don't know your Steam Api Key perform these steps:
Step 1: go to https://steamcommunity.com/dev/apikey
Step 2: Copy your SteamApi key to clipboard";
        public const string ApiCallParamSteamIdTutorial = @"
If you don't know your steam id perform these steps:
Step 1: go to https://steamcommunity.com/
Step 2: Login
Step 3: In upper right corner click on your profile's icon
Step 4: Copy to clipboard your SteamId from browser address bar
https://steamcommunity.com/profiles/{YOUR_STEAM_ID}/
";

        public string PluginKey => PluginKeyConst;
        public string MarketplaceKey => PluginMarketplaceKeyConst;

        public bool UseDefaultBrowserLayout => false;

        public string DisplayName => "Steam";

        public string Description => "Import games from Steam";

        public string IconUrl => "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/512px-Steam_icon_logo.svg.png";

        public string ImportDescription => "You need to provide SteamApiKey and SteamId ";

        public string ImportSourceUrl => string.Empty;

        public bool UseDefaultHttpClientLayout => true;

        public Dictionary<string, string> UseHttpClientCallParams => new Dictionary<string, string>
        {
            { ApiCallParamSteamApiKey, ApiCallParamSteamApiKeyTutorial },
            { ApiCallParamSteamId, ApiCallParamSteamIdTutorial}
        };

        public string ImportApiUrl => "https://api.steampowered.com/IPlayerService/GetOwnedGames/v1/?key={steamApiKey}&steamid={steamId}&include_appinfo=1&include_played_free_games=1";

        public string ImportApiCallMethod => "GET";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddKeyedSingleton<IAssetsImporter, SteamAssetsImporter>(PluginKey);
            services.AddSingleton<ISteamService, SteamService>();
        }
    }
}
