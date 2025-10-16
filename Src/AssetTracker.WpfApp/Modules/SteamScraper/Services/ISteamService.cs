using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.WpfApp.Modules.SteamScraper.Services
{
    public interface ISteamService
    {
        Task<List<SteamGame>> GetSteamGamesAsync(string steamApiKey, string steamId);
    }

    public class SteamGame
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public int PlaytimeForever { get; set; }
        public string ImgIconUrl { get; set; }
    }
    public class SteamGamesResponse
    {
        public ResponseData Response { get; set; }
    }

    public class ResponseData
    {
        public int GameCount { get; set; }
        public List<SteamGame> Games { get; set; }
    }
}
