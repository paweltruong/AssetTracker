using AssetTracker.Core.Models;
using AssetTracker.Core.Services;

namespace AssetTracker.Application.Services
{
    public class FilesAssetDatabase : IAssetDatabase
    {
        private readonly IList<OwnedAsset> _assets = new List<OwnedAsset>();

        public event EventHandler AssetDatabaseChanged;

        public Task AddAssetsAsync(IEnumerable<OwnedAsset> assets)
        {
            foreach (var asset in assets)
            {
                if (!_assets.Contains(asset))
                {
                    _assets.Add(asset);
                }
            }
            RaiseAssetDatabaseChangedEvent();
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

        public Task<IEnumerable<OwnedAsset>> GetAssetsForMarketplaceAsync(string marketplaceKey)
        {
            return Task.FromResult(_assets.Where(a=>a.MarketplaceKey.Equals(marketplaceKey)));
        }

        public void RaiseAssetDatabaseChangedEvent()
        {
            AssetDatabaseChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
