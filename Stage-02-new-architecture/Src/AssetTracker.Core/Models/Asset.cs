using AssetTracker.Core.Models.Enums;

namespace AssetTracker.Core.Models
{
    public class Asset : IEquatable<Asset>
    {
        private Asset()
        {

        }

        public Asset(
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
            HashSet<string> searchKeywords
            )
        {
            MarketplaceUid = marketplaceUid;
            Name = name;
            AssetType = assetType;
            Tags = tags;
            ImageUrl = imageUrl;
            AssetUrl = assetUrl;
            Developers = developers;
            Publishers = publishers;
            SourcePluginKey = sourcePluginKey;
            MarketplaceKey = marketplaceKey;
            SearchKeywords = searchKeywords;
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
        public HashSet<string> SearchKeywords { get; set; } = new HashSet<string>();

        public static Asset Create(string name, AssetType assetType, HashSet<string> tags, string imageUrl,
            IList<Publisher> publishers,
            IList<Developer> developers,
            string assetUrl,
            HashSet<string> searchKeywords)
        {
            var asset = new Asset()
            {
                Name = name,
                AssetType = assetType,
                Tags = tags,
                ImageUrl = imageUrl,
                Developers = developers,
                Publishers = publishers,
                AssetUrl = assetUrl,
                SearchKeywords = searchKeywords
            };
            return asset;
        }

        public bool Equals(Asset? other)
        {
            if (other == null)
                return false;

            return MarketplaceUid == other.MarketplaceUid && MarketplaceKey == other.MarketplaceKey;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Asset);
        }

        public override int GetHashCode()
        {
            return $"{MarketplaceKey}_{MarketplaceUid}".GetHashCode();
        }

        public bool Differs(Asset asset)
        {
            if (MarketplaceUid != asset.MarketplaceUid) return true;
            if (Name != asset.Name) return true;
            if (AssetType != asset.AssetType) return true;
            if (Tags.SetEquals(asset.Tags)) return true;
            if (ImageUrl != asset.ImageUrl) return true;
            if (Publishers.SequenceEqual(asset.Publishers)) return true;
            if (Developers.SequenceEqual(asset.Developers)) return true;
            if (AssetUrl != asset.AssetUrl) return true;
            if (SourcePluginKey != asset.SourcePluginKey) return true;
            if (MarketplaceKey != asset.MarketplaceKey) return true;
            if (SearchKeywords != asset.SearchKeywords) return false;

            return false;
        }
    }
}
