using AssetTracker.WpfApp.Modules.SteamScraper.Models;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Services
{
    public interface ISteamService
    {
        Task<List<SteamGame>> GetSteamGamesAsync(string steamApiKey, string steamId);
    }

}
