using AssetTracker.AssetsImporter.SyntyStore.Definitions;
using HtmlAgilityPack;
using System.Net;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using Microsoft.Data.Sqlite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AssetTracker.AssetsImporter.SyntyStore.Services
{
    //public class SyntyStoreScraper : IDisposable
    //{
    //    private readonly HttpClient _httpClient;
    //    private readonly CookieContainer _cookieContainer = new();
    //    private readonly string _chromeProfilePath;

    //    public SyntyStoreScraper(string chromeProfilePath = null)
    //    {
    //        // default Chrome profile path for current user
    //        //_chromeProfilePath ??= chromeProfilePath ?? Path.Combine(
    //        //    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    //        //    "Google",
    //        //    "Chrome",
    //        //    "User Data",
    //        //    "Default");
    //        _chromeProfilePath ??= chromeProfilePath ?? Path.Combine(
    //            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    //            "Google",
    //            "Chrome",
    //            "User Data",
    //            "Profile 1",
    //            "Network");

    //        _chromeProfilePath = chromeProfilePath ?? _chromeProfilePath;

    //        // Setup HttpClient with cookie container
    //        var handler = new HttpClientHandler
    //        {
    //            CookieContainer = _cookieContainer,
    //            AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
    //            AllowAutoRedirect = true
    //        };

    //        _httpClient = new HttpClient(handler);
    //        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117 Safari/537.36");
    //        _httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

    //        // Try to import cookies from Chrome for syntystore.com host
    //        ImportChromeCookiesToContainer("syntystore.com").GetAwaiter().GetResult();
    //    }

    //    public void Dispose()
    //    {
    //        _httpClient?.Dispose();
    //    }

    //    // PUBLIC: scrape all product pages, following "Next" until no next link
    //    public async Task<List<ProductItem>> ScrapeAllProductsAsync(string startingUrl = "https://syntystore.com/apps/downloads/orders")
    //    {
    //        var products = new List<ProductItem>();
    //        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
    //        string currentUrl = startingUrl;

    //        while (!string.IsNullOrEmpty(currentUrl) && !visited.Contains(currentUrl))
    //        {
    //            visited.Add(currentUrl);
    //            Console.WriteLine($"Fetching: {currentUrl}");
    //            var html = await GetStringWithRetriesAsync(currentUrl);
    //            if (html == null) break;

    //            var doc = new HtmlDocument();
    //            doc.LoadHtml(html);

    //            // extract product links
    //            var nodes = doc.DocumentNode.SelectNodes("//a[contains(@class,'sky-pilot-list-item')]");
    //            if (nodes != null)
    //            {
    //                foreach (var a in nodes)
    //                {
    //                    string href = a.GetAttributeValue("href", "");
    //                    string absoluteHref = MakeAbsoluteUrl(currentUrl, href);
    //                    string title = a.SelectSingleNode(".//div[contains(@class,'sky-pilot-file-heading')]")?.InnerText?.Trim() ?? "";
    //                    string thumbnail = a.SelectSingleNode(".//img[contains(@class,'sky-pilot-product-thumbnail')]")?.GetAttributeValue("src", "") ?? "";

    //                    products.Add(new ProductItem
    //                    {
    //                        Title = title,
    //                        DownloadUrl = absoluteHref,
    //                        ThumbnailUrl = thumbnail
    //                    });
    //                }
    //            }

    //            // find next page link inside pagination
    //            var nextNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class,'sky-pilot-pagination')]//span[contains(@class,'next')]/a");
    //            if (nextNode == null)
    //            {
    //                // no next link -> done
    //                break;
    //            }

    //            string nextHref = nextNode.GetAttributeValue("href", "");
    //            if (string.IsNullOrEmpty(nextHref)) break;
    //            currentUrl = MakeAbsoluteUrl(currentUrl, nextHref);

    //            // polite delay
    //            await Task.Delay(300);
    //        }

    //        return products;
    //    }

    //    // Helper: performs GET with retries and returns string content
    //    private async Task<string> GetStringWithRetriesAsync(string url, int attempts = 3)
    //    {
    //        for (int i = 0; i < attempts; i++)
    //        {
    //            try
    //            {
    //                var resp = await _httpClient.GetAsync(url);
    //                if (resp.IsSuccessStatusCode)
    //                {
    //                    return await resp.Content.ReadAsStringAsync();
    //                }

    //                Console.WriteLine($"HTTP {(int)resp.StatusCode} for {url}");
    //            }
    //            catch (Exception ex)
    //            {
    //                Console.WriteLine($"Request error: {ex.Message}");
    //            }

    //            await Task.Delay(500);
    //        }

    //        return null;
    //    }

    //    // Build absolute URL from a base
    //    private static string MakeAbsoluteUrl(string baseUrl, string href)
    //    {
    //        if (Uri.TryCreate(href, UriKind.Absolute, out var abs)) return abs.ToString();
    //        if (href.StartsWith("//")) return $"https:{href}";
    //        if (href.StartsWith("/"))
    //        {
    //            var baseUri = new Uri(baseUrl);
    //            return $"{baseUri.Scheme}://{baseUri.Host}{href}";
    //        }

    //        // fallback: combine
    //        try
    //        {
    //            var baseUri = new Uri(baseUrl);
    //            return new Uri(baseUri, href).ToString();
    //        }
    //        catch
    //        {
    //            return href;
    //        }
    //    }

    //    // ---------------------- Chrome cookie import logic ----------------------

    //    private async Task ImportChromeCookiesToContainer(string domain)
    //    {
    //        try
    //        {
    //            var cookies = await ReadCookiesFromChromeProfileAsync(domain);
    //            foreach (var c in cookies)
    //            {
    //                var cookie = new Cookie(c.Name, c.Value, c.Path ?? "/", c.Domain.StartsWith(".") ? c.Domain.Substring(1) : c.Domain)
    //                {
    //                    Expires = c.Expires.HasValue ? DateTimeOffset.FromUnixTimeSeconds(c.Expires.Value).UtcDateTime : DateTime.MinValue,
    //                    Secure = c.Secure,
    //                    HttpOnly = c.HttpOnly
    //                };

    //                _cookieContainer.Add(new Uri("https://" + cookie.Domain), cookie);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Console.WriteLine("Could not import Chrome cookies: " + ex.Message);
    //        }
    //    }

    //    // Cookie DTO
    //    private class ChromeCookie
    //    {
    //        public string Name { get; set; }
    //        public string Value { get; set; }
    //        public string Domain { get; set; }
    //        public string Path { get; set; }
    //        public bool Secure { get; set; }
    //        public bool HttpOnly { get; set; }
    //        public long? Expires { get; set; } // epoch seconds
    //    }

    //    // Core: read cookies for host from Chrome SQLite cookies store
    //    private async Task<List<ChromeCookie>> ReadCookiesFromChromeProfileAsync(string hostKey)
    //    {
    //        var cookies = new List<ChromeCookie>();

    //        // Paths
    //        string cookiesFile = Path.Combine(_chromeProfilePath, "Cookies");
    //        string localStateFile = Path.Combine(Path.GetDirectoryName(_chromeProfilePath), "Local State");

    //        if (!File.Exists(cookiesFile))
    //            throw new FileNotFoundException("Chrome Cookies DB not found at: " + cookiesFile);

    //        // Copy DB to avoid lock
    //        string tempDb = Path.Combine(Path.GetTempPath(), "Cookies_copy_" + Guid.NewGuid().ToString("N") + ".db");
    //        File.Copy(cookiesFile, tempDb, true);

    //        // Read encryption key from Local State (if present)
    //        byte[] masterKey = null;
    //        if (File.Exists(localStateFile))
    //        {
    //            try
    //            {
    //                string localStateJson = await File.ReadAllTextAsync(localStateFile);
    //                using var doc = JsonDocument.Parse(localStateJson);
    //                if (doc.RootElement.TryGetProperty("os_crypt", out var osCrypt) &&
    //                    osCrypt.TryGetProperty("encrypted_key", out var encKeyEl))
    //                {
    //                    string encKeyBase64 = encKeyEl.GetString();
    //                    var encryptedKeyWithPrefix = Convert.FromBase64String(encKeyBase64);
    //                    // Newer Chrome stores DPAPI-wrapped AES key with "DPAPI" prefix — strip that if present
    //                    const int dpapiPrefixLen = 5; // "DPAPI"
    //                    if (encryptedKeyWithPrefix.Length > dpapiPrefixLen && Encoding.ASCII.GetString(encryptedKeyWithPrefix, 0, dpapiPrefixLen) == "DPAPI")
    //                    {
    //                        var dpapiWrapped = new byte[encryptedKeyWithPrefix.Length - dpapiPrefixLen];
    //                        Array.Copy(encryptedKeyWithPrefix, dpapiPrefixLen, dpapiWrapped, 0, dpapiWrapped.Length);
    //                        masterKey = ProtectedData.Unprotect(dpapiWrapped, null, DataProtectionScope.CurrentUser);
    //                    }
    //                    else
    //                    {
    //                        // older style - try DPAPI anyway
    //                        try { masterKey = ProtectedData.Unprotect(encryptedKeyWithPrefix, null, DataProtectionScope.CurrentUser); } catch { masterKey = null; }
    //                    }
    //                }
    //            }
    //            catch { masterKey = null; }
    //        }

    //        // Query DB for cookies matching hostKey
    //        try
    //        {
    //            using var conn = new SqliteConnection($"Data Source={tempDb};Mode=ReadOnly;");
    //            conn.Open();

    //            // The host_key column stores things like ".syntystore.com" or "syntystore.com"
    //            using var cmd = conn.CreateCommand();
    //            cmd.CommandText = "SELECT name, value, encrypted_value, host_key, path, expires_utc, is_secure, is_httponly FROM cookies WHERE host_key LIKE $host OR host_key LIKE $dothost";
    //            cmd.Parameters.AddWithValue("$host", "%" + hostKey + "%");
    //            cmd.Parameters.AddWithValue("$dothost", "%." + hostKey + "%");

    //            using var reader = cmd.ExecuteReader();
    //            while (reader.Read())
    //            {
    //                string name = reader.GetString(0);
    //                string value = reader.IsDBNull(1) ? "" : reader.GetString(1);
    //                byte[] encryptedValue = reader.IsDBNull(2) ? Array.Empty<byte>() : (byte[])reader.GetFieldValue<byte[]>(2);
    //                string host = reader.IsDBNull(3) ? "" : reader.GetString(3);
    //                string path = reader.IsDBNull(4) ? "/" : reader.GetString(4);
    //                long expiresUtc = reader.IsDBNull(5) ? 0 : reader.GetInt64(5); // Chrome stores as microseconds since 1601-01-01
    //                bool isSecure = !reader.IsDBNull(6) && reader.GetBoolean(6);
    //                bool isHttpOnly = !reader.IsDBNull(7) && reader.GetBoolean(7);

    //                string decryptedValue = value;

    //                if ((string.IsNullOrEmpty(value) || reader.GetString(1).Length == 0) && encryptedValue != null && encryptedValue.Length > 0)
    //                {
    //                    // decrypt encrypted_value
    //                    decryptedValue = DecryptChromiumCookie(encryptedValue, masterKey);
    //                }

    //                // convert expires: Chrome uses WebKit/Windows epoch: microseconds since 1601-01-01 UTC
    //                long? expiresUnix = null;
    //                if (expiresUtc > 0)
    //                {
    //                    try
    //                    {
    //                        // Convert to seconds since epoch 1970
    //                        // chromeExpiresUtc in microseconds since 1601-01-01
    //                        var epoch1601 = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    //                        var expiryDt = epoch1601.AddTicks(expiresUtc * 10); // microseconds -> ticks (1 tick = 100ns = 10 microseconds)
    //                        var unix = new DateTimeOffset(expiryDt).ToUnixTimeSeconds();
    //                        expiresUnix = unix;
    //                    }
    //                    catch { expiresUnix = null; }
    //                }

    //                cookies.Add(new ChromeCookie
    //                {
    //                    Name = name,
    //                    Value = decryptedValue,
    //                    Domain = host,
    //                    Path = path,
    //                    Secure = isSecure,
    //                    HttpOnly = isHttpOnly,
    //                    Expires = expiresUnix
    //                });
    //            }
    //        }
    //        finally
    //        {
    //            try { File.Delete(tempDb); } catch { /* ignore */ }
    //        }

    //        return cookies;
    //    }

    //    // Decrypt cookie encrypted_value bytes
    //    // If masterKey is present (Local State), uses AES-GCM with that key for values starting with "v10"
    //    // Otherwise, falls back to DPAPI ProtectedData.
    //    private static string DecryptChromiumCookie(byte[] encryptedValue, byte[] masterKey)
    //    {
    //        const string v10 = "v10";
    //        try
    //        {
    //            string prefix = Encoding.ASCII.GetString(encryptedValue, 0, Math.Min(encryptedValue.Length, 3));
    //            if (prefix == v10 && masterKey != null)
    //            {
    //                // new AES-GCM: encryptedValue = "v10" + nonce(12) + ciphertext + tag(16)
    //                // layout: b"v10" || nonce(12) || ciphertext || tag(16)
    //                int nonceStart = 3;
    //                byte[] nonce = new byte[12];
    //                Array.Copy(encryptedValue, nonceStart, nonce, 0, nonce.Length);

    //                int cipherStart = nonceStart + nonce.Length;
    //                int tagLength = 16;
    //                int cipherLen = encryptedValue.Length - cipherStart - tagLength;
    //                byte[] ciphertext = new byte[cipherLen];
    //                Array.Copy(encryptedValue, cipherStart, ciphertext, 0, cipherLen);
    //                byte[] tag = new byte[tagLength];
    //                Array.Copy(encryptedValue, cipherStart + cipherLen, tag, 0, tagLength);

    //                var plaintext = new byte[cipherLen];
    //                // AesGcm in .NET expects ciphertext + tag concatenated; but we can pass tag separately using overload
    //                // Use System.Security.Cryptography.AesGcm
    //                using var aes = new AesGcm(masterKey);
    //                try
    //                {
    //                    aes.Decrypt(nonce, Combine(ciphertext, tag), Span<byte>.Empty, plaintext); // wrong overload -> use proper approach below
    //                }
    //                catch
    //                {
    //                    // The above attempt might be incorrect; do by passing ciphertext and tag together:
    //                    var cipherPlusTag = Combine(ciphertext, tag);
    //                    var outBuf = new byte[ciphertext.Length];
    //                    aes.Decrypt(nonce, cipherPlusTag, outBuf, null);
    //                    return Encoding.UTF8.GetString(outBuf);
    //                }

    //                return Encoding.UTF8.GetString(plaintext);
    //            }
    //            else
    //            {
    //                // Older DPAPI-only cookie: use ProtectedData.Unprotect on entire blob
    //                try
    //                {
    //                    var decrypted = ProtectedData.Unprotect(encryptedValue, null, DataProtectionScope.CurrentUser);
    //                    return Encoding.UTF8.GetString(decrypted);
    //                }
    //                catch
    //                {
    //                    // best-effort fallback to try skipping DPAPI header if present
    //                    try
    //                    {
    //                        // Sometimes there may be a prefix; search for UTF8 text inside
    //                        var possible = Encoding.UTF8.GetString(encryptedValue);
    //                        if (!string.IsNullOrWhiteSpace(possible) && possible.Contains("=") || possible.Contains("value"))
    //                            return possible;
    //                    }
    //                    catch { }
    //                    return "";
    //                }
    //            }
    //        }
    //        catch
    //        {
    //            return "";
    //        }
    //    }

    //    private static byte[] Combine(byte[] a1, byte[] a2)
    //    {
    //        var r = new byte[a1.Length + a2.Length];
    //        Buffer.BlockCopy(a1, 0, r, 0, a1.Length);
    //        Buffer.BlockCopy(a2, 0, r, a1.Length, a2.Length);
    //        return r;
    //    }
    //}


    //=================================



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
