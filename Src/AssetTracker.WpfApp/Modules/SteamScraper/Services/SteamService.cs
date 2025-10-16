using System.Net.Http;
using System.Text.Json;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Services
{
    public class SteamService : ISteamService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.steampowered.com";

        public SteamService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<List<SteamGame>> GetSteamGamesAsync(string steamApiKey, string steamId)
        {
            var url = $"/IPlayerService/GetOwnedGames/v1/?key={steamApiKey}&steamid={steamId}&include_appinfo=1&include_played_free_games=1";

            var response = await _httpClient.GetAsync(url);

            //simulate delay for testing
            //await Task.Delay(5000);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SteamGamesResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result?.Response?.Games ?? new List<SteamGame>();
        }
    }
}
