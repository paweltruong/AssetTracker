using AssetTracker.WpfApp.Common.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using System.Windows;
using System.Windows.Controls;

namespace AssetTracker.WpfApp.Common.Views
{
    /// <summary>
    /// Interaction logic for DefaultBrowserAssetsImporterView.xaml
    /// </summary>
    public partial class DefaultBrowserAssetsImporterView : UserControl, IScraperServiceMainView
    {
        private readonly ILogger _logger;

        public DefaultBrowserAssetsImporterView(ILogger<DefaultBrowserAssetsImporterView> logger)
        {
            _logger = logger;
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

                    var env = await CoreWebView2Environment.CreateAsync();
                    await Browser.EnsureCoreWebView2Async(env);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "WebView2 init error");
                //MessageBox.Show($"WebView2 init error: {ex.Message}");
            }

            if (DataContext is DefaultBrowserAssetsImporterViewModel viewModel)
            {
                viewModel.SetupBrowser(Browser);
            }
        }

        private async void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    await Browser.EnsureCoreWebView2Async();
            //    Browser.Source = new Uri("https://syntystore.com/");
            //}
            //catch (Exception ex)
            //{
            //    //MessageBox.Show("WebView2 init failed: " + ex.Message);
            //}
        }
    }
}
