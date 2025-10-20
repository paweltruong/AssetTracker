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
                return await new Task<IEnumerable<OwnedAsset>>(() =>
                {
                    List<OwnedAsset> ownedAssets = new List<OwnedAsset>
                    {
                        new OwnedAsset
                        {
                             Name = "POLYGON - Casino",
                            //public AssetType AssetType { get; set; }
                            //public HashSet<string>? Tags { get; set; }
                            //public string? ImageUrl { get; set; }
                            //public IList<Publisher> Publishers { get; set; }
                            //public IList<Developer> Developers { get; set; }
                            //public string? AssetUrl { get; set; }
                            MarketplaceAccountId = "paweltruong@o2.pl",
                            MarketplaceName = "Synty Store",
                            MarketplaceUrl = "https://syntystore.com/collections/polygon-sci-fi-city-pack/products/polygon-casino",
                        }
                    };
                    return ownedAssets.AsEnumerable();
                });
            }
            return await new Task<IEnumerable<OwnedAsset>>(() => Enumerable.Empty<OwnedAsset>());
        }
    }
}
