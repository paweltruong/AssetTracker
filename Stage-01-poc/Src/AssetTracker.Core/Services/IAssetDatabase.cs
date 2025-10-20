using AssetTracker.Core.Models;

namespace AssetTracker.Core.Services
{
    public interface IAssetDatabase
    {
        Task<IEnumerable<OwnedAsset>> FindAsync(Asset asset, CancellationToken cancellationToken);
        Task FindAssetAsync();
    }
}
