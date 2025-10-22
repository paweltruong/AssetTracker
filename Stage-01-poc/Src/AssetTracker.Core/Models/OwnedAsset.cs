using AssetTracker.Core.Models.Enums;
using System.Xml.Linq;

namespace AssetTracker.Core.Models
{
    public class OwnedAsset : Asset
    {
        public OwnedAsset(string name,
                    AssetType assetType,
                    string imageUrl,
                    string assetUrl,
                    string sourcePluginKey,
                    string marketplaceKey) : base(name,
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
    }
}
