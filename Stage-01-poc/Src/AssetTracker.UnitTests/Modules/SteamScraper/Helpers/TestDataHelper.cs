using AssetTracker.WpfApp.Modules.SteamScraper.Models;
using System.Text.Json;

namespace AssetTracker.UnitTests.Modules.SteamScraper.Helpers
{
    public static class TestDataHelper
    {
        public static string GetSteamGamesRequestUrl(string steamApiKey, string steamId) => $"/IPlayerService/GetOwnedGames/v1/?key={steamApiKey}&steamid={steamId}&include_appinfo=1&include_played_free_games=1";

        public static List<SteamGame> CreateTestGames()
        {
            return new List<SteamGame>
            {
                new SteamGame { AppId = 730, Name = "Counter-Strike: Global Offensive", PlaytimeForever = 1440, ImgIconUrl = "csgo_icon" },
                new SteamGame { AppId = 570, Name = "Dota 2", PlaytimeForever = 2880, ImgIconUrl = "dota2_icon" }
            };
        }

        public static string CreateSteamApiResponse()
        {
            var response = new
            {
                response = new
                {
                    game_count = 3,
                    games = new[]
                    {
                        new { appid = 730, name = "Counter-Strike: Global Offensive", playtime_forever = 1440, img_icon_url = "csgo_icon" },
                        new { appid = 570, name = "Dota 2", playtime_forever = 2880, img_icon_url = "dota2_icon" }
                    }
                }
            };

            return JsonSerializer.Serialize(response);
        }

        public static string CreateInvalidSteamApiResponse()
        {
            return @"{
    ""response"": {
        ""game_count"": 320,
        ""games"": [
            {
                ""appid"": 240,
                ""name"": ""Counter-Strike: Source"",
                ""playtime_forever"": 913,
                ""img_icon_url"": ""9052fa60c496a1c03383b27687ec50f4bf0f0e10"",
                ""has_community_visible_stats"": true,
                ""playtime_windows_forever"": 0,
                ""playtime_mac_forever"": 0,
                ""playtime_linux_forever"": 0,
                ""playtime_deck_forever"": 0,
                ""rtime_last_played"": 86400,
                ""content_descriptorids"": [
                    2,
                    5
                ],
                ""playtime_disconnected"": 0
            },
			{
                ""appid"": 570,
                ""name"": ""Dota 2"",
                ""playtime_forever"": 197230,
                ""img_icon_url"": ""0bbb630d63262dd66d2fdd0f7d37e8661a410075"",
                ""playtime_windows_forever"": 16774,
                ""playtime_mac_forever"": 0,
                ""playtime_linux_forever"": 0,
                ""playtime_deck_forever"": 0,
                ""rtime_last_played"": 1735767884,
                ""content_descriptorids"": [
                    5
                ],
                ""playtime_disconnected"": 2
            }
			]
    }
}";
        }
    }
}
