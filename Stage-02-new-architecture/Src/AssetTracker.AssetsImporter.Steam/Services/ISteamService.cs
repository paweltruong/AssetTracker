using AssetTracker.AssetsImporter.Steam.Definitions;

namespace AssetTracker.AssetsImporter.Steam.Services
{
    /// <summary>
    /// Interface for Steam service to fetch Steam games.
    /// </summary>
    public interface ISteamService
    {
        Task<List<SteamGame>> GetSteamGamesAsync(string steamApiKey, string steamId, CancellationToken cancellationToken = default);
        /// <summary>
        ///  Just beware that Steam imposes a rate limit of 200 requests per 5 minutes.
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<SteamAppData> GetSteamGameDetailsAsync(int appId, CancellationToken cancellationToken = default);
    }

}
