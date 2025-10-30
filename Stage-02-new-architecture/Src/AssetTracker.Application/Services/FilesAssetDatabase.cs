using AssetTracker.Core.Models;
using AssetTracker.Core.Services;

namespace AssetTracker.Application.Services
{
    public class FilesAssetDatabase : IAssetDatabase
    {
        private readonly ICacheManager _cacheManager;

        private IList<OwnedAsset> _assets = new List<OwnedAsset>();
        private readonly object _lock = new object();

        /// <summary>
        /// MarketplaceKey and DateTime
        /// </summary>
        Dictionary<string, DateTime?> _pluginImportDates = new Dictionary<string, DateTime?>();


        public FilesAssetDatabase(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public bool IsDirty { get; private set; }

        public event EventHandler AssetDatabaseChanged;
        public event EventHandler AssetDatabaseLoaded;

        public Task AddAssetsAsync(IEnumerable<OwnedAsset> assets)
        {
            lock (_lock)
            {
                foreach (var asset in assets)
                {
                    var existingAsset = _assets.FirstOrDefault(a => a.Equals(asset));
                    var indexOfExistingAsset = _assets.IndexOf(existingAsset);
                    if (existingAsset == null)
                    {
                        asset.IsDirty = true;
                        _assets.Add(asset);
                        IsDirty = true;
                    }
                    else if(existingAsset.Differs(asset))
                    {
                        asset.IsDirty = true;
                        existingAsset = asset;
                        _assets[indexOfExistingAsset] = asset;
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

        public async Task<IEnumerable<OwnedAssetMatch>> FindAsync(Asset asset, CancellationToken cancellationToken)
        {
            List<OwnedAssetMatch> matches = new List<OwnedAssetMatch>();
            var potentialMatches = _assets.Where(
                a => a.SearchKeywords != null && asset.SearchKeywords != null 
                && a.SearchKeywords.Intersect(asset.SearchKeywords).Any());
            foreach (var match in potentialMatches)
            {
                matches.Add(new OwnedAssetMatch(
                    searchKeywords: asset.SearchKeywords,
                    match: match,
                    matchPercentage: (double)match.SearchKeywords.Intersect(asset.SearchKeywords).Count() / (double)asset.SearchKeywords.Count
                    ));
            }
            return matches;
        }

        public async Task<IEnumerable<OwnedAsset>> GetAllAssetsAsync()
        {
            lock (_lock)
            {
                return _assets;
            }
        }

        public async Task LoadAllAssetsAsync()
        {
            var assetDatabaseDatas = await _cacheManager.LoadAllMarketplaceDataAsync();
            _pluginImportDates = assetDatabaseDatas.Select(adb =>
                new KeyValuePair<string, DateTime?>(adb.MarketplaceKey, adb.LastImportData)).ToDictionary();
            _assets = assetDatabaseDatas.SelectMany(add => add.OwnedAssets).ToList();
            RaiseAssetDatabaseLoadedEvent();
        }

        public async Task<IEnumerable<OwnedAsset>> GetAssetsForMarketplaceAsync(string marketplaceKey)
        {
            lock (_lock)
            {
                return _assets.Where(a => a.MarketplaceKey.Equals(marketplaceKey));
            }
        }

        public IEnumerable<OwnedAsset> GetAssetsForMarketplace(string marketplaceKey)
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
        public void RaiseAssetDatabaseLoadedEvent()
        {
            AssetDatabaseLoaded?.Invoke(this, EventArgs.Empty);
        }

        public async Task SaveAsync()
        {
            var assetTmp = new List<OwnedAsset>(_assets);
            await _cacheManager.SaveOwnedAssetsAsync(_assets, _pluginImportDates);
            //TODO:add missing plugin to _pluginImportDates
            assetTmp.ForEach(a => a.IsDirty = false);
            IsDirty = false;
            _assets = assetTmp;
            RaiseAssetDatabaseChangedEvent();
        }

        public DateTime? GetImportDate(string pluginMarketplaceKey)
        {
            if (_pluginImportDates.ContainsKey(pluginMarketplaceKey))
                return _pluginImportDates[pluginMarketplaceKey];
            return null;
        }

        public async Task SaveImportParametersAsync(string pluginKey, Dictionary<string, string> parameterValues)
        {
            await _cacheManager.SaveImportParametersAsync(pluginKey, parameterValues);
        }

        public Dictionary<string, string> LoadImportParameters(string pluginKey)
        {
            return _cacheManager.LoadImportParameters(pluginKey);
        }

        public bool HasAnyAssetsFromMarketplace(string pluginMarketplaceKey)
        {
            return _assets.Any(a => a.MarketplaceKey == pluginMarketplaceKey);
        }
    }
}
