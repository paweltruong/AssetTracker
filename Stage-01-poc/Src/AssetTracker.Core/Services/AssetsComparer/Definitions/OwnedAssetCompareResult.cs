using AssetTracker.Core.Models;

namespace AssetTracker.Core.Services.AssetsComparer.Definitions
{
    public class OwnedAssetCompareResult
    {
        public bool WasSuccessful { get; set; }
        public IEnumerable<OwnedAsset> MatchingOwnedAssets { get; set; }
    }
}
