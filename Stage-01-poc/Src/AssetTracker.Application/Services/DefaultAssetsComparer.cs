using AssetTracker.Core.Models;
using AssetTracker.Core.Models.Enums;
using AssetTracker.Core.Services;
using AssetTracker.Core.Services.AssetsComparer;
using AssetTracker.Core.Services.AssetsComparer.Definitions;
using Microsoft.Extensions.Logging;

namespace AssetTracker.WpfApp.Common.Services
{
    public class DefaultAssetsComparer : IAssetsComparer
    {
        IAssetDatabase _assetDatabase;
        ILogger<DefaultAssetsComparer> _logger;
        public DefaultAssetsComparer(IAssetDatabase assetDatabase, ILogger<DefaultAssetsComparer> logger)
        {
            _assetDatabase = assetDatabase;
            _logger = logger;
        }

        public async Task<OwnedAssetCompareResult> CompareAssetAsync(Asset asset, CancellationToken cancellationToken = default)
        {
            OwnedAssetCompareResult result = new OwnedAssetCompareResult
            {
                WasSuccessful = false,
                MatchingOwnedAssets = new List<OwnedAsset>(),
            };

            try
            {
                result.MatchingOwnedAssets = await _assetDatabase.FindAsync(asset, cancellationToken);   
                result.WasSuccessful = true;
            }
            catch (OperationCanceledException ocex)
            {
                _logger.LogWarning(ocex, "Asset comparison was cancelled.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during asset comparison.");
            }

            return result;
        }

        public async Task CompareAsync(CancellationToken cancellationToken = default)
        {
        }
    }
}
