using AssetTracker.Core.Models;

namespace AssetTracker.Core.Services.AssetsResolver.Definitions
{
    public class AssetsResolverResult
    {
        public bool WasSuccessful { get; set; }
        public IEnumerable<Asset> Items { get; set; }
    }
}
