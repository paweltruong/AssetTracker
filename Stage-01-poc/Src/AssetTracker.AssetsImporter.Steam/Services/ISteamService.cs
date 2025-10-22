using AssetTracker.AssetsImporter.Steam.Definitions;
using AssetTracker.Core.Models;

namespace AssetTracker.AssetsImporter.Steam.Services
{
    /// <summary>
    /// Interface for Steam service to fetch Steam games.
    /// </summary>
    public interface ISteamService
    {
        Task<List<SteamGame>> GetSteamGamesAsync(string steamApiKey, string steamId, CancellationToken cancellationToken = default);
    }

}
