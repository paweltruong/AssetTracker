using AssetTracker.Core.Models.Enums;
using System.Runtime.InteropServices;

namespace AssetTracker.Core.Models
{
    public class Asset
    {
        private Asset()
        {

        }

        public Asset(string marketplaceUid,
            string name,
            AssetType assetType,
            string imageUrl,
            string assetUrl,
            string sourcePluginKey,
            string marketplaceKey)
        {
            MarketplaceUid = marketplaceUid;
            Name = name;
            AssetType = assetType;
            ImageUrl = imageUrl;
            AssetUrl = assetUrl;
            SourcePluginKey = sourcePluginKey;
            MarketplaceKey = marketplaceKey;
        }


        public string MarketplaceUid { get; set; }
        public string Name { get; set; }
        public AssetType AssetType { get; set; }
        public HashSet<string>? Tags { get; set; }
        public string? ImageUrl { get; set; }
        public IList<Publisher> Publishers { get; set; }
        public IList<Developer> Developers { get; set; }
        public string? AssetUrl { get; set; }
        public string SourcePluginKey { get; set; }
        public string MarketplaceKey { get; set; }

        public static Asset Create(string name, AssetType assetType, HashSet<string> tags, string imageUrl,
            IList<Publisher> publishers,
            IList<Developer> developers,
            string assetUrl)
        {
            var asset = new Asset()
            {
                Name = name,
                AssetType = assetType,
                Tags = tags,
                ImageUrl = imageUrl,
                Developers = developers,
                Publishers = publishers,
                AssetUrl = assetUrl
            };
            return asset;
        }
    }
}
