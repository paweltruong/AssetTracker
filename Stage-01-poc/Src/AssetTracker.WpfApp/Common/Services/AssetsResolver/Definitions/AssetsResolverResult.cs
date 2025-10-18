using AssetTracker.WpfApp.Common.Models;

namespace AssetTracker.WpfApp.Common.Services.AssetsResolver.Definitions
{
    public class AssetsResolverResult
    {
        public bool WasSuccessful { get; set; }
        public IEnumerable<AssetItem> Items { get; set; }
    }
}
