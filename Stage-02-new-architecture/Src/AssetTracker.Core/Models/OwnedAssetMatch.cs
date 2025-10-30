namespace AssetTracker.Core.Models
{
    public class OwnedAssetMatch
    {
        public HashSet<string> SearchKeywords { get; set; }
        public OwnedAsset Match { get; set; }
        public double MatchPercentage { get; set; }

        public OwnedAssetMatch(HashSet<string> searchKeywords, OwnedAsset match, double matchPercentage)
        {
            SearchKeywords = searchKeywords;
            Match = match;
            MatchPercentage = matchPercentage;
        }
    }
}
