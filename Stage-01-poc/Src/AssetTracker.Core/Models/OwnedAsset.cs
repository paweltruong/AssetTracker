namespace AssetTracker.Core.Models
{
    public class OwnedAsset : Asset
    {
        public string MarketplaceName { get; set; }
        public string MarketplaceAccountId { get; set; }
        public string MarketplaceUrl { get; set; }
    }
}
