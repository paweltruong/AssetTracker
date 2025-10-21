namespace AssetTracker.AssetsImporter.SyntyStore.Definitions
{
    public class ProductItem
    {
        public string Title { get; set; }
        public string DownloadUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string FullDownloadUrl => $"https://syntystore.com{DownloadUrl}";
    }
}
