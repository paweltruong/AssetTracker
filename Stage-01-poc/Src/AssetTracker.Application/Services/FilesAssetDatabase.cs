using AssetTracker.Core.Models;
using AssetTracker.Core.Services;

namespace AssetTracker.Application.Services
{
    public class FilesAssetDatabase : IAssetDatabase
    {
        public Task FindAssetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OwnedAsset>> FindAsync(Asset asset, CancellationToken cancellationToken)
        {
            if (asset.Name.Equals("POLYGON - Casino"))
            {
                // Return the result directly - no Task creation needed
                return new List<OwnedAsset>
                {
                    new OwnedAsset
                    {
                        Name = "POLYGON - Casino",
                        MarketplaceAccountId = "paweltruong@o2.pl",
                        MarketplaceName = "Synty Store",
                        MarketplaceUrl = "https://syntystore.com/collections/polygon-sci-fi-city-pack/products/polygon-casino",
                    }
                };
            }

            return Enumerable.Empty<OwnedAsset>();
        }
    }
}
