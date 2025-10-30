namespace AssetTracker.AssetsImporter.Steam.Definitions
{
    public class SteamGame
    {
        public int AppId {  get; set; }

        public string Name { get; set; }

        public int PlaytimeForever { get; set; }

        public string ImgIconUrl { get; set; }

        // Calculated properties for easier binding
        public string FullImageUrl =>
            $"https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/{AppId}/header.jpg";

        public string PlaytimeHours => $"{PlaytimeForever} hrs";
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
