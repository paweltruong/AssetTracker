using AssetTracker.Core.Models;
using System.Collections.ObjectModel;

namespace AssetTracker.Core.Services
{
    public interface IAssetDatabase
    {
        Task<IEnumerable<OwnedAsset>> FindAsync(Asset asset, CancellationToken cancellationToken);
        Task FindAssetAsync();
        Task AddAssetsAsync(IEnumerable<OwnedAsset> assets);

        event EventHandler AssetDatabaseChanged;

        void RaiseAssetDatabaseChangedEvent();
        Task<IEnumerable<OwnedAsset>> GetAssetsForMarketplaceAsync(string marketplaceKey);
    }
}
