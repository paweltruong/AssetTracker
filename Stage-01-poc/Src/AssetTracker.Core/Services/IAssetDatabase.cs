using AssetTracker.Core.Models;

namespace AssetTracker.Core.Services
{
    public interface IAssetDatabase
    {
        Task<IEnumerable<OwnedAsset>> FindAsync(Asset asset, CancellationToken cancellationToken);
        Task FindAssetAsync();
        Task AddAssetsAsync(IEnumerable<OwnedAsset> assets);

        event EventHandler AssetDatabaseChanged;
        event EventHandler AssetDatabaseLoaded;

        void RaiseAssetDatabaseChangedEvent();
        Task<IEnumerable<OwnedAsset>> GetAllAssetsAsync();
        Task<IEnumerable<OwnedAsset>> GetAssetsForMarketplaceAsync(string marketplaceKey);
        IEnumerable<OwnedAsset> GetAssetsForMarketplace(string marketplaceKey);
        Task LoadAllAssetsAsync();
        Task SaveAsync();
        DateTime? GetImportDate(string pluginMarketplaceKey);
        Task SaveImportParametersAsync(string pluginKey, Dictionary<string, string> parameterValues);
        Dictionary<string, string> LoadImportParameters(string pluginKey);
        bool HasAnyAssetsFromMarketplace(string pluginMarketplaceKey);

        bool IsDirty { get; }
    }
}
