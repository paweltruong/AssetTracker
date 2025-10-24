using AssetTracker.Core.Models.Enums;
using System.Text.Json.Serialization;

namespace AssetTracker.Core.Models
{
    public class OwnedAsset : Asset
    {
        public OwnedAsset(
                    string marketplaceUid,
                    string name,
                    AssetType assetType,
                    string imageUrl,
                    string assetUrl,
                    string sourcePluginKey,
                    string marketplaceKey) : base(
                        marketplaceUid,
                        name,
                        assetType,
                        imageUrl,
                        assetUrl,
                        sourcePluginKey,
                        marketplaceKey)
        {

        }

        public string MarketplaceName { get; set; }
        public string MarketplaceAccountId { get; set; }
        public string MarketplaceUrl { get; set; }

        [JsonIgnore]
        /// <summary>
        /// Is object has changed that was not applied into the persistend <see cref="AssetTracker.Core.Services.IAssetDatabase">
        /// </summary>
        public bool IsDirty { get; set; }
    }
}
