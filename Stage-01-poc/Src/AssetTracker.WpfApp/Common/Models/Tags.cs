using AssetTracker.WpfApp.Common.Models.Enums;

namespace AssetTracker.WpfApp.Common.Models
{
    public static class Tags
    {
        public const string GameTag = "Game";
        public const string SoftwareTag = "Software";
        public const string BookTag = "Book";

        /// <summary>
        /// Get default tags for given asset type for Humble Bundle assets
        /// </summary>
        /// <param name="assetType"></param>
        /// <returns></returns>
        public static string GetDefaultTags(AssetType assetType)
        {
            return assetType switch
            {
                AssetType.Game => GameTag,
                AssetType.Book => BookTag,
                AssetType.Software => SoftwareTag,
                _ => string.Empty
            };
        }
    }
}
