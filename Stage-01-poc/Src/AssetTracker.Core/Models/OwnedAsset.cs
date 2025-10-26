using AssetTracker.Core.Models.Enums;
using System.Text.Json.Serialization;

namespace AssetTracker.Core.Models
{
    public class OwnedAsset : Asset, IEquatable<OwnedAsset>
    {
        public OwnedAsset(
           string marketplaceUid,
           string name,
           AssetType assetType,
           HashSet<string> tags,
           string imageUrl,
           string assetUrl,
           IList<Publisher> publishers,
           IList<Developer> developers,
           string sourcePluginKey,
           string marketplaceKey,
           string marketplaceName,
           string marketplaceAccountId,
           string marketplaceUrl) : base(
                        marketplaceUid,
                        name,
                        assetType,
                        tags,
                        imageUrl,
                        assetUrl,
                        publishers,
                        developers,
                        sourcePluginKey,
                        marketplaceKey)
        {
            MarketplaceName = marketplaceName;
            MarketplaceAccountId = marketplaceAccountId;
            MarketplaceUrl = marketplaceUrl;
        }

        public string MarketplaceName { get; set; }
        public string MarketplaceAccountId { get; set; }
        public string MarketplaceUrl { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Is object has changed that was not applied into the persistend <see cref="AssetTracker.Core.Services.IAssetDatabase">
        /// </summary>
        public bool IsDirty { get; set; }

        public bool Differs(OwnedAsset asset)
        {
            if (base.Differs(asset)) return true;

            if (MarketplaceName != asset.MarketplaceName) return true;
            if (MarketplaceAccountId != asset.MarketplaceAccountId) return true;
            if (MarketplaceUrl != asset.MarketplaceUrl) return true;

            return false;
        }

        public bool Equals(OwnedAsset? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            // Compare based on what makes an asset unique
            return MarketplaceUid == other.MarketplaceUid && MarketplaceKey == other.MarketplaceKey;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as OwnedAsset);
        }
    }
}
