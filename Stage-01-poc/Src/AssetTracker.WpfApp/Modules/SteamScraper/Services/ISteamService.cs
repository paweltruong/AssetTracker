using AssetTracker.WpfApp.Modules.SteamScraper.Models;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Services
{
    /// <summary>
    /// Interface for Steam service to fetch Steam games.
    /// </summary>
    public interface ISteamService
    {
        Task<List<SteamGame>> GetSteamGamesAsync(string steamApiKey, string steamId, CancellationToken cancellationToken = default);
    }

}
