using AssetTracker.Core.Services.AssetsResolver.Definitions;

namespace AssetTracker.Core.Services.AssetsResolver
{
    /// <summary>
    /// Interface for resolving assets from a given URL.
    /// </summary>
    public interface IAssetsResolver
    {
        Task<AssetsResolverResult> ResolveAssetFromUrlAsync(string url, CancellationToken cancellationToken = default);
    }
}
