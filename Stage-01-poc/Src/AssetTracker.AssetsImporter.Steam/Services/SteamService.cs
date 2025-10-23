using AssetTracker.Application.Services;
using AssetTracker.AssetsImporter.Steam.Definitions;
using System.Text.Json;

namespace AssetTracker.AssetsImporter.Steam.Services
{
    public class SteamService : ISteamService
    {
        private readonly IMyHttpClient _httpClient;
        public const string BaseUrl = "https://api.steampowered.com";

        public SteamService(IMyHttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<List<SteamGame>> GetSteamGamesAsync(string steamApiKey, string steamId, CancellationToken cancellationToken = default)
        {
            var url = $"/IPlayerService/GetOwnedGames/v1/?key={steamApiKey}&steamid={steamId}&include_appinfo=1&include_played_free_games=1";

            var response = await _httpClient.GetAsync(url, cancellationToken);

            //simulate delay for testing
            //await Task.Delay(5000, cancellationToken);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<SteamGamesResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Response?.Games ?? new List<SteamGame>();
        }
    }
}
