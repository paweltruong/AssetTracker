namespace AssetTracker.AssetsImporter.SyntyStore.Definitions
{
    public class ScrapingResult
    {
        public List<ProductItem> Products { get; set; } = new List<ProductItem>();
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string NextPageUrl { get; set; }
    }
}
