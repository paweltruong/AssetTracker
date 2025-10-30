using AssetTracker.Core.Models;

namespace AssetTracker.Application.Services
{
    public interface ICacheManager
    {
        string CacheFolderPath { get; }

        Task SaveOwnedAssetsAsync(IEnumerable<OwnedAsset> data, Dictionary<string, DateTime?> oldImportDates);
        Task<IEnumerable<AssetDatabaseData>> LoadAllMarketplaceDataAsync();
        Task SaveImportParametersAsync(string pluginKey, Dictionary<string, string> parameterValues);
        Dictionary<string, string> LoadImportParameters(string pluginKey);
    }
}
