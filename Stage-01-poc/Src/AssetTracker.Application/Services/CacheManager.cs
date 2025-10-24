using AssetTracker.Core.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AssetTracker.Application.Services
{
    public class CacheManager : ICacheManager
    {
        private readonly ILogger _logger;
        private readonly string _cacheFolderPath;
        private readonly string _assetRegisterFolderPath;

        const string Company = "PawciuDev";
        const string AppName = "AssetTracker";
        const string CacheDirName = "Cache";
        const string AssetRegisterDirName = "AssetRegister";

        public CacheManager(ILogger<CacheManager> logger)
        {
            _logger = logger;

            // Get the local AppData folder for your application
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _cacheFolderPath = Path.Combine(appDataPath, Company, AppName, CacheDirName);
            _assetRegisterFolderPath = Path.Combine(_cacheFolderPath, AssetRegisterDirName);

            // Ensure the directory exists
            Directory.CreateDirectory(_cacheFolderPath);
            Directory.CreateDirectory(_assetRegisterFolderPath);
        }

        public async Task SaveOwnedAssetsAsync(IEnumerable<OwnedAsset> data)
        {
            var groupedAssets = data.GroupBy(oa=>oa.MarketplaceKey);

            await Parallel.ForEachAsync(groupedAssets, async (group, cancellationToken) =>
            {
                try
                {
                    var databaseData = new AssetDatabaseData
                    {
                        MarketplaceKey = group.Key,
                        LastImportData = DateTime.UtcNow,
                        OwnedAssets = group.ToArray() // Materialize the group
                    };

                    await SaveMarketplaceDataAsync(databaseData);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error saving assets for {group.Key}: {ex.Message}");
                }
            });
        }

        public async Task SaveMarketplaceDataAsync(AssetDatabaseData data)
        {
            try
            {
                string fileName = $"{SanitizeFileName(data.MarketplaceKey)}.json";
                string filePath = Path.Combine(_assetRegisterFolderPath, fileName);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                string json = JsonSerializer.Serialize(data, options);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving marketplace data for {data.MarketplaceKey}: {ex.Message}");
                throw;
            }
        }

        private string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            return string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries))
                         .TrimEnd('.');
        }

        public async Task SaveToCacheAsync<T>(string cacheKey, IEnumerable<T> data)
        {
            try
            {

                string filePath = GetCacheFilePath(cacheKey);
                string json = JsonSerializer.Serialize(data);

                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cache save error: {ex.Message}");
            }
        }

        // Read data from cache
        public async Task<T> ReadFromCacheAsync<T>(string cacheKey)
        {
            try
            {
                string filePath = GetCacheFilePath(cacheKey);

                if (!File.Exists(filePath))
                    return default;

                string json = await File.ReadAllTextAsync(filePath);
                var cacheItem = JsonSerializer.Deserialize<CacheItem<T>>(json);

                // Check if cache is expired
                if (cacheItem.ExpiresAt.HasValue && DateTime.UtcNow > cacheItem.ExpiresAt.Value)
                {
                    DeleteCacheFile(cacheKey);
                    return default;
                }

                return cacheItem.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cache read error: {ex.Message}");
                return default;
            }
        }

        // Delete specific cache file
        public void DeleteCacheFile(string cacheKey)
        {
            try
            {
                string filePath = GetCacheFilePath(cacheKey);
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cache delete error: {ex.Message}");
            }
        }

        public async Task<IEnumerable<AssetDatabaseData>> LoadAllMarketplaceDataAsync()
        {
            var results = new List<AssetDatabaseData>();

            if (!Directory.Exists(_assetRegisterFolderPath))
                return results;

            var jsonFiles = Directory.GetFiles(_assetRegisterFolderPath, "*.json");

            await Parallel.ForEachAsync(jsonFiles, async (filePath, cancellationToken) =>
            {
                try
                {
                    string json = await File.ReadAllTextAsync(filePath);
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };

                    var data = JsonSerializer.Deserialize<AssetDatabaseData>(json, options);
                    if (data != null)
                    {
                        lock (results)
                        {
                            results.Add(data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error loading file {Path.GetFileName(filePath)}: {ex.Message}");
                }
            });

            return results;
        }

        public IEnumerable<string> GetAvailableMarketplaces()
        {
            if (!Directory.Exists(_assetRegisterFolderPath))
                return Enumerable.Empty<string>();

            return Directory.GetFiles(_assetRegisterFolderPath, "*.json")
                           .Select(Path.GetFileNameWithoutExtension);
        }



        // Get cache file size info
        public (long SizeInBytes, int FileCount) GetCacheInfo()
        {
            if (!Directory.Exists(_cacheFolderPath))
                return (0, 0);

            var files = Directory.GetFiles(_cacheFolderPath);
            long totalSize = 0;

            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                totalSize += fileInfo.Length;
            }

            return (totalSize, files.Length);
        }

        private string GetCacheFilePath(string cacheKey)
        {
            // Sanitize the cache key to be filesystem-safe
            string safeFileName = string.Join("_", cacheKey.Split(Path.GetInvalidFileNameChars()));
            return Path.Combine(_cacheFolderPath, $"{safeFileName}.cache");
        }
    }

    // Cache item structure
    public class CacheItem<T>
    {
        public T Data { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
