using AssetTracker.AssetsImporter.SyntyStore.Definitions;
using HtmlAgilityPack;
using System.Net;
using System.Web;

namespace AssetTracker.AssetsImporter.SyntyStore.Services
{
    public class SyntyStoreScraper
    {
        public const string PaginationUrlRoot = "https://syntystore.com";
        public SyntyStoreScraper()
        {
        }

        public async Task<ScrapingResult> ScrapeAllProductsAsync(string url, string htmlContent, CancellationToken cancellationToken = default)
        {
            var result = new ScrapingResult();
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            try
            {
                // Get the current page
                var pageResult = await ScrapePageAsync(htmlDoc);
                if (!pageResult.Success)
                {
                    result.Success = false;
                    result.ErrorMessage = $"Failed to scrape page {url}: {pageResult.ErrorMessage}";
                    return result;
                }

                // Add products from this page
                result.Products.AddRange(pageResult.Products);

                // Get next page URL
                result.NextPageUrl = await GetNextPageUrlAsync(htmlDoc);

                // Small delay to be respectful to the server

                result.Success = true;
                return result;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Unexpected error: {ex.Message}";
                return result;
            }
        }

        private async Task<ScrapingResult> ScrapePageAsync(HtmlDocument htmlDoc)
        {
            var result = new ScrapingResult();

            // Find all product links
            var productNodes = htmlDoc.DocumentNode.SelectNodes("//a[@class='sky-pilot-list-item']");

            if (productNodes != null)
            {
                foreach (var node in productNodes)
                {
                    var product = new ProductItem
                    {
                        DownloadUrl = node.GetAttributeValue("href", ""),
                        Title = ExtractProductTitle(node),
                        ThumbnailUrl = ExtractThumbnailUrl(node)
                    };

                    if (!string.IsNullOrEmpty(product.DownloadUrl))
                    {
                        result.Products.Add(product);
                    }
                }
            }

            result.Success = true;
            return result;
        }

        private string ExtractProductTitle(HtmlNode productNode)
        {
            var titleNode = productNode.SelectSingleNode(".//div[@class='sky-pilot-file-heading']");
            return titleNode?.InnerText?.Trim() ?? "Unknown Product";
        }

        private string ExtractThumbnailUrl(HtmlNode productNode)
        {
            var imgNode = productNode.SelectSingleNode(".//img[@class='sky-pilot-product-thumbnail']");
            return imgNode?.GetAttributeValue("src", "") ?? "";
        }

        private async Task<string> GetNextPageUrlAsync(HtmlDocument htmlDoc)
        {
            // Look for next page link
            var nextPageNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='sky-pilot-pagination']//span[@class='next']/a");

            if (nextPageNode != null)
            {
                string relativeUrlHtmlString = nextPageNode.GetAttributeValue("href", "");
                if (!string.IsNullOrEmpty(relativeUrlHtmlString))
                {
                    string decodedString = WebUtility.HtmlDecode(relativeUrlHtmlString);

                    return $"{PaginationUrlRoot}{decodedString}";
                }
            }

            return null; // No next page found
        }

        public void Dispose()
        {
        }
    }
}
