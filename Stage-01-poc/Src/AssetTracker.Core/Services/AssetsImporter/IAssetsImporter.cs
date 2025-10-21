using AssetTracker.Core.Models;

namespace AssetTracker.Core.Services.AssetsImporter
{
    public interface IAssetsImporter
    {

        Task<IEnumerable<OwnedAsset>> ImportAssetsAsync(CancellationToken cancellationToken = default);
    }
}
