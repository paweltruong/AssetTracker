using AssetTracker.Core.Models.Enums;

namespace AssetTracker.Core.Models
{
    public class Asset
    {
        public string Name { get; set; }
        public AssetType AssetType { get; set; }
        public HashSet<string>? Tags { get; set; }
        public string? ImageUrl { get; set; }
        public IList<Publisher> Publishers { get; set; }
        public IList<Developer> Developers { get; set; }
        public string? AssetUrl { get; set; }
        public string SourcePluginKey { get; set; }
    }
}
