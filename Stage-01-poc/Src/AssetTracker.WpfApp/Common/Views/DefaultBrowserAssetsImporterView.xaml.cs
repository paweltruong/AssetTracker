using AssetTracker.WpfApp.Modules.SteamScraper.ViewModels;
using Microsoft.Web.WebView2.Core;
using System.Net.Http;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using AssetTracker.WpfApp.Common.ViewModels;

namespace AssetTracker.WpfApp.Common.Views
{
    /// <summary>
    /// Interaction logic for DefaultBrowserAssetsImporterView.xaml
    /// </summary>
    public partial class DefaultBrowserAssetsImporterView : UserControl, IScraperServiceMainView
    {
        private HttpClient _client;
        private CookieContainer _cookieContainer;

        public DefaultBrowserAssetsImporterView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Only initialize once
                if (Browser.CoreWebView2 == null)
                {
                    if (DataContext is DefaultBrowserAssetsImporterViewModel viewModel)
                    {
                        viewModel.SetupBrowser(Browser);
                    }

                    var env = await CoreWebView2Environment.CreateAsync();
                    await Browser.EnsureCoreWebView2Async(env);

                    // Now it’s safe to navigate or use WebView2
                    Browser.Source = new Uri("https://syntystore.com/account/login");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"WebView2 init error: {ex.Message}");
            }
        }

        private async void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Browser.EnsureCoreWebView2Async();
                Browser.Source = new Uri("https://syntystore.com/");
            }
            catch (Exception ex)
            {
                //MessageBox.Show("WebView2 init failed: " + ex.Message);
            }
        }

        private async void ScrapeButton_Click(object sender, RoutedEventArgs e)
        {
            var cookies = await GetCookiesAsync("https://syntystore.com");
            if (cookies == null)
            {
                MessageBox.Show("No cookies found. Please log in first.");
                return;
            }

            // Build HttpClient using WebView2 cookies
            _cookieContainer = new CookieContainer();
            foreach (var cookie in cookies)
            {
                var c = new Cookie(cookie.Name, cookie.Value, cookie.Path, cookie.Domain);
                _cookieContainer.Add(c);
            }

            var handler = new HttpClientHandler { CookieContainer = _cookieContainer };
            _client = new HttpClient(handler);

            // Now call the orders page
            var html = await _client.GetStringAsync("https://syntystore.com/apps/downloads/orders");

            // You can now parse the HTML as before
            File.WriteAllText("synty_orders.html", html);
            MessageBox.Show("Scraped HTML saved to synty_orders.html");
        }


        private async void ScrapeButton2_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is DefaultBrowserAssetsImporterViewModel viewModel)
            {
                await viewModel.BeginScrapingInBrowserAsync(Browser);
            }
        }

        private async Task<CoreWebView2Cookie[]> GetCookiesAsync(string url)
        {
            if (Browser.CoreWebView2 == null)
                return null;

            var cookieManager = Browser.CoreWebView2.CookieManager;
            var cookies = await cookieManager.GetCookiesAsync(url);
            return cookies.ToArray();
        }
    }
}
