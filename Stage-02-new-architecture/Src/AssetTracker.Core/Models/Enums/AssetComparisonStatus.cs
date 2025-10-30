namespace AssetTracker.Core.Models.Enums
{
    public enum AssetComparisonStatus
    {
        Unknown = 0,
        /// <summary>
        /// Looking for a match
        /// </summary>
        Processing,
        NotExists,
        Exists,
        Error,
    }
}
