using AssetTracker.Core.Models;
using AssetTracker.Core.Services;

namespace AssetTracker.Application.Services
{
    public class FilesAssetDatabase : IAssetDatabase
    {

        private readonly IList<OwnedAsset> _assets = new List<OwnedAsset>();
        private readonly object _lock = new object();

        public bool IsDirty { get; private set; }

        public event EventHandler AssetDatabaseChanged;

        public Task AddAssetsAsync(IEnumerable<OwnedAsset> assets)
        {
            lock (_lock)
            {
                foreach (var asset in assets)
                {
                    if (!_assets.Contains(asset))
                    {
                        asset.IsDirty = true;
                        _assets.Add(asset);
                        IsDirty = true;
                    }
                }

                RaiseAssetDatabaseChangedEvent();
            }
            return Task.CompletedTask;
        }

        public Task FindAssetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OwnedAsset>> FindAsync(Asset asset, CancellationToken cancellationToken)
        {
            //if (asset.Name.Equals("POLYGON - Casino"))
            //{
            //    // Return the result directly - no Task creation needed
            //    return new List<OwnedAsset>
            //    {
            //        new OwnedAsset(
            //            name: "POLYGON - Casino",
            //            marketplaceAccountId: "paweltruong@o2.pl",
            //            MarketplaceName = "Synty Store",
            //            MarketplaceUrl = "https://syntystore.com/collections/polygon-sci-fi-city-pack/products/polygon-casino",
            //            SourcePluginKey
            //        }
            //    };
            //}

            return Enumerable.Empty<OwnedAsset>();
        }

        public async Task<IEnumerable<OwnedAsset>> GetAllAssetsAsync()
        {
            lock (_lock)
            {
                return _assets;
            }
        }

        public async Task<IEnumerable<OwnedAsset>> GetAssetsForMarketplaceAsync(string marketplaceKey)
        {
            lock (_lock)
            {
                return _assets.Where(a => a.MarketplaceKey.Equals(marketplaceKey));
            }
        }

        public void RaiseAssetDatabaseChangedEvent()
        {
            AssetDatabaseChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
