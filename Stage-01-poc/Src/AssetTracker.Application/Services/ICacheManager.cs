using AssetTracker.Core.Models;

namespace AssetTracker.Application.Services
{
    public interface ICacheManager
    {
        Task SaveOwnedAssetsAsync(IEnumerable<OwnedAsset> data);
        Task<IEnumerable<AssetDatabaseData>> LoadAllMarketplaceDataAsync();
    }
}
