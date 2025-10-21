using AssetTracker.AssetsImporter.SyntyStore.Definitions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AssetTracker.AssetsImporter.SyntyStore.Services
{
    public class SyntyStoreScraper
    {
        private readonly HttpClient _httpClient;

        public SyntyStoreScraper()
        {
            // Create HttpClient with Chrome-like headers
            var handler = new HttpClientHandler
            {
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
        }

        public async Task<ScrapingResult> ScrapeAllProductsAsync()
        {
            var result = new ScrapingResult();

            try
            {
                string currentPageUrl = "https://syntystore.com/apps/downloads/orders";
                int pageCount = 0;
                const int maxPages = 1000; // Safety limit to prevent infinite loops

                while (!string.IsNullOrEmpty(currentPageUrl) && pageCount < maxPages)
                {
                    pageCount++;

                    // Get the current page
                    var pageResult = await ScrapePageAsync(currentPageUrl);
                    if (!pageResult.Success)
                    {
                        result.Success = false;
                        result.ErrorMessage = $"Failed to scrape page {pageCount}: {pageResult.ErrorMessage}";
                        return result;
                    }

                    // Add products from this page
                    result.Products.AddRange(pageResult.Products);

                    // Get next page URL
                    currentPageUrl = await GetNextPageUrlAsync(currentPageUrl);

                    // Small delay to be respectful to the server
                    await Task.Delay(1000);
                }

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

        private async Task<ScrapingResult> ScrapePageAsync(string url)
        {
            var result = new ScrapingResult();

            try
            {
                // Make the HTTP request
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    result.Success = false;
                    result.ErrorMessage = $"HTTP error: {response.StatusCode}";
                    return result;
                }

                var htmlContent = await response.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

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
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                return result;
            }
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

        private async Task<string> GetNextPageUrlAsync(string currentPageUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(currentPageUrl);
                if (!response.IsSuccessStatusCode)
                    return null;

                var htmlContent = await response.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                // Look for next page link
                var nextPageNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='sky-pilot-pagination']//span[@class='next']/a");

                if (nextPageNode != null)
                {
                    string relativeUrl = nextPageNode.GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(relativeUrl))
                    {
                        return $"https://syntystore.com{relativeUrl}";
                    }
                }

                return null; // No next page found
            }
            catch
            {
                return null;
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
