using AssetTracker.UnitTests.Modules.HumbleBundle.AssetsResolver.Helpers;
using AssetTracker.UnitTests.Modules.HumbleBundle.AssetsResolver.TestData;
using AssetTracker.WpfApp.Common.Services;
using AssetTracker.WpfApp.Modules.HumbleBundle.AssetsResolver;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace AssetTracker.UnitTests.Modules.HumbleBundle.AssetsResolver
{
    public class HumbleBundleAssetsResolverTests
    {
        private readonly Mock<IMyHttpClient> _httpClient;
        private readonly Mock<ILogger<HumbleBundleAssetsResolver>> _logger = new Mock<ILogger<HumbleBundleAssetsResolver>>();

        public HumbleBundleAssetsResolverTests()
        {
            _httpClient = new Mock<IMyHttpClient>();
        }

        HumbleBundleAssetsResolver BuildSut()
        {
            return new HumbleBundleAssetsResolver(_httpClient.Object, _logger.Object);
        }

        //This cannot by properly displayed in the test runner due to the number of test cases
        //[Theory]
        //[MemberData(nameof(HumbleBundleTestData.BundleTestCases), MemberType = typeof(HumbleBundleTestData))]
        //public void GetAssetDataFromHtmlSource_ValidInputs_ReturnsExpectedAssets(TestCaseData testCase)
        //{
        //    // Arrange
        //    var htmlSource = TestDataHelpers.LoadTestHtml(testCase.HtmlFileName);
        //    var sut = BuildSut();

        //    // Act
        //    var actualAssets = sut.GetAssetDataFromHtmlSource(testCase.BundleType, testCase.IsBundle, ref htmlSource).ToList();

        //    // Assert
        //    var testd = string.Join("\r\n", actualAssets.Select(x => x.Name));
        //    actualAssets.Should().NotBeNull();
        //    actualAssets.Should().HaveCount(testCase.ExpectedAssets.Count,
        //        $"expected {testCase.ExpectedAssets.Count} assets from {testCase.HtmlFileName}");
        //    TestDataHelpers.AssertAssetItemsEqual(testCase.ExpectedAssets, actualAssets);
        //}

        [Fact]        
        public void GetAssetDataFromHtmlSource_ValidSynty5Bundle_ReturnsExpectedAssets()
        {
            Generic_GetAssetDataFromHtmlSource_ValidInputs_ReturnsExpectedAssets(HumbleBundleTestData.Software_Best_Synty_Game_Dev_Assets_5);
        }

        void Generic_GetAssetDataFromHtmlSource_ValidInputs_ReturnsExpectedAssets(TestCaseData testCase)
        {
            // Arrange
            var htmlSource = TestDataHelpers.LoadTestHtml(testCase.HtmlFileName);
            var sut = BuildSut();

            // Act
            var actualAssets = sut.GetAssetDataFromHtmlSource(testCase.BundleType, testCase.IsBundle, ref htmlSource).ToList();

            // Assert
            actualAssets.Should().NotBeNull();
            actualAssets.Should().HaveCount(testCase.ExpectedAssets.Count,
                $"expected {testCase.ExpectedAssets.Count} assets from {testCase.HtmlFileName}");
            TestDataHelpers.AssertAssetItemsEqual(testCase.ExpectedAssets, actualAssets);
        }
    }
}
