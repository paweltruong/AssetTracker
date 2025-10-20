using AssetTracker.Core.Models.Enums;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;

namespace AssetTracker.UnitTests.Modules.HumbleBundle.AssetsResolver.TestData
{
    public static class HumbleBundleTestData
    {
        //Used for MemberData but cannot be properlu displayed in test runner
        //public static TheoryData<TestCaseData> BundleTestCases => new()
        //{
        //    Software_Best_Synty_Game_Dev_Assets_5,
        //    Software_30000_Loops_For_30
        //};

        public static TestCaseData Software_Best_Synty_Game_Dev_Assets_5 = new TestCaseData
        {
            HtmlFileName = "source_software_best-synty-game-dev-assets-5.html",
            BundleType = AssetType.Software,
            IsBundle = true,
            ExpectedAssets = new List<ExpectedAssetItem>
            {
                new ExpectedAssetItem { Name = "POLYGON - Sci-Fi City Pack", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "INTERFACE - Apocalypse HUD", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "POLYGON - Kaiju", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "POLYGON - Casino", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "POLYGON - Street Racer", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "POLYGON - Mech Pack", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "Simple Cars", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "Simple Props/Items/Icons", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "POLYGON - Arid Desert - Nature Biome", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "Simple Military", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "Simple Dungeons", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "Simple Zombies", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "$10 Voucher", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "Simple People 1", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "Simple Town", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "POLYGON - Horror Carnival", Type = AssetType.Software },
                new ExpectedAssetItem { Name = "ANIMATION - Base Locomotion", Type = AssetType.Software }
            }
        };
        public static TestCaseData Software_30000_Loops_For_30 = new TestCaseData
        {
            HtmlFileName = "source_software_30000-loops-for-30-software.html",
            BundleType = AssetType.Software,
            IsBundle = true,
            ExpectedAssets = new List<ExpectedAssetItem>
            {
                new ExpectedAssetItem { Name = "Synty Studio - Polygon GameDev Assets", Type = AssetType.Software, Url = "https://example.com/asset1" },
                new ExpectedAssetItem { Name = "Character Pack 1", Type = AssetType.Software, Url = "https://example.com/asset2" },
                new ExpectedAssetItem { Name = "Environment Pack", Type = AssetType.Software, Url = "https://example.com/asset3" }
            }
        };

    }

    public class TestCaseData
    {
        public string DisplayName { get; set; }
        public string HtmlFileName { get; set; }
        public AssetType BundleType { get; set; }
        public bool IsBundle { get; set; }
        public List<ExpectedAssetItem> ExpectedAssets { get; set; } = new();

        public override string ToString() => DisplayName ?? HtmlFileName;
    }

    public class ExpectedAssetItem
    {
        public string Name { get; set; }
        public AssetType Type { get; set; }
        public string Url { get; set; }

        public HashSet<string> Tags { get; set; } = new();

        public PublisherItem[] Publishers { get; set; } = Array.Empty<PublisherItem>();
        public DeveloperItem[] Developers { get; set; } = Array.Empty<DeveloperItem>();
    }
}

