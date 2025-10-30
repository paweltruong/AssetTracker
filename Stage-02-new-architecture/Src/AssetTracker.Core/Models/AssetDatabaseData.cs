using System.Text.Json.Serialization;

namespace AssetTracker.Core.Models
{
    public class AssetDatabaseData
    {
        [JsonPropertyName("marketplaceKey")]
        public string MarketplaceKey { get; set; }
        [JsonPropertyName("lastImportData")]
        public DateTime LastImportData { get; set; }
        [JsonPropertyName("ownedAssets")]
        public IEnumerable<OwnedAsset> OwnedAssets { get; set; }
        [JsonPropertyName("assetCount")]
        public int AssetCount => OwnedAssets?.Count() ?? 0;
    }
}
