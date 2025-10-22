using AssetTracker.Application.Services;
using AssetTracker.AssetsResolver.HumbleBundle.Definitions;
using AssetTracker.Core.Models;
using AssetTracker.Core.Models.Enums;
using AssetTracker.Core.Services.AssetsResolver;
using AssetTracker.Core.Services.AssetsResolver.Definitions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AssetTracker.AssetsResolver.HumbleBundle
{
    public class HumbleBundleAssetsResolver : IAssetsResolver
    {
        const string HumbleBundleGameBundleUrlPart = "https://www.humblebundle.com/games/";
        const string HumbleBundleSoftwareBundleUrlPart = "https://www.humblebundle.com/software/";
        const string HumbleBundleBookBundleUrlPart = "https://www.humblebundle.com/books/";
        const string HumbleBundleStoreUrlPart = "https://www.humblebundle.com/store/";

        private readonly IMyHttpClient _httpClient;
        private readonly ILogger<HumbleBundleAssetsResolver> _logger;

        public HumbleBundleAssetsResolver(IMyHttpClient myHttpClient, ILogger<HumbleBundleAssetsResolver> logger)
        {
            _httpClient = myHttpClient;
            _logger = logger;
        }

        public async Task<AssetsResolverResult> ResolveAssetFromUrlAsync(string url, CancellationToken cancellationToken = default)
        {
            //https://www.humblebundle.com/software/best-synty-game-dev-assets-5-software?hmb_source=&hmb_medium=product_tile&hmb_campaign=mosaic_section_1_layout_index_2_layout_type_threes_tile_index_2_c_bestsyntygamedevassets5_softwarebundle
            AssetType linkAssetType;
            bool isBundle;
            bool wasSuccessful = false;
            IEnumerable<Asset> assets = new List<Asset>();

            if (!TryGetAssetTypeFromUrl(url, out linkAssetType, out isBundle))
            {
                return new AssetsResolverResult
                {
                    WasSuccessful = wasSuccessful,
                    Items = assets
                };
            }

            await Task.Delay(3000, cancellationToken); // Simulate some async work


            try
            {
                // Fetch the HTML content
                string htmlContent = await _httpClient.GetStringAsync(url);

                assets = GetAssetDataFromHtmlSource(linkAssetType, isBundle, ref htmlContent);

                _logger.LogTrace(string.Join("\r\n", assets.Select(x => x.Name)));
                wasSuccessful = true;
            }
            catch (OperationCanceledException ocex)
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {

            }


            return new AssetsResolverResult
            {
                WasSuccessful = wasSuccessful,
                Items = assets
            };
        }

        public bool TryGetAssetTypeFromUrl(string url, out AssetType assetType, out bool isBundle)
        {
            assetType = AssetType.Unknown;
            isBundle = false;

            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            if (url.StartsWith(HumbleBundleBookBundleUrlPart))
            {
                assetType = AssetType.Book;
                isBundle = true;
                return true;
            }
            if (url.StartsWith(HumbleBundleGameBundleUrlPart))
            {
                assetType = AssetType.Game;
                isBundle = true;
                return true;
            }
            if (url.StartsWith(HumbleBundleStoreUrlPart))
            {
                assetType = AssetType.Game;
                isBundle = false;
                return true;
            }
            if (url.StartsWith(HumbleBundleSoftwareBundleUrlPart))
            {
                assetType = AssetType.Software;
                isBundle = true;
                return true;
            }
            return false;
        }

        public IEnumerable<Asset> GetAssetDataFromHtmlSource(AssetType assetType, bool isBundle, ref string htmlSource)
        {
            // Parse HTML using HtmlAgilityPack
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlSource);

            if (isBundle)
            {

                // Find the script tag with id="webpack-bundle-page-data"
                var scriptNode = htmlDocument.DocumentNode.SelectSingleNode("//script[@id='webpack-bundle-page-data']");

                if (scriptNode != null)
                {
                    // Extract the JSON content from the script tag
                    string jsonContent = scriptNode.InnerText.Trim();

                    // Parse the JSON string to JsonDocument
                    JsonDocument jsonData = JsonDocument.Parse(jsonContent);
                    var bundleData = jsonData.Deserialize<WebPackBundlePageData>();
                    return AssetItemBuilder.BuildAssetItems(bundleData);
                }
                else
                {
                    Console.WriteLine("Script tag with id 'webpack-bundle-page-data' not found.");
                }
            }
            else
            {
                // Handle non-bundle case if needed
            }
            return new List<Asset>();
        }
    }
}
