using AssetTracker.Core.Models;
using AssetTracker.Core.Services.AssetsComparer.Definitions;

namespace AssetTracker.Core.Services.AssetsComparer
{
    public interface IAssetsComparer
    {
        Task CompareAsync(CancellationToken cancellationToken = default);
        Task<OwnedAssetCompareResult> CompareAssetAsync(Asset asset, CancellationToken cancellationToken = default);
    }
}
