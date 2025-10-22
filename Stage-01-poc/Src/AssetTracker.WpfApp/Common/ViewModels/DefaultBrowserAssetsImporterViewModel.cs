using AssetTracker.Core.Models;
using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.AssetsImporter.Definitions;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class DefaultBrowserAssetsImporterViewModel : ViewModelBase
    {
        private IAssetsImporterPlugin _plugin;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAssetsImporter _assetImporter;

        private CancellationTokenSource _lastCancellationTokenSource;
        public ICommand OpenLinkCommand { get; }
        public IAsyncRelayCommand StartCommand { get; }
        public IAsyncRelayCommand StopCommand { get; }

        Microsoft.Web.WebView2.Wpf.WebView2 _browser;

        public DefaultBrowserAssetsImporterViewModel(IEventAggregator eventAggregator, IAssetsImporterPlugin plugin, IAssetsImporter assetImporter)
        {
            _eventAggregator = eventAggregator;
            _plugin = plugin;
            _assetImporter = assetImporter;

            OpenLinkCommand = new RelayCommand<string>(OpenLink);
            StartCommand = new AsyncRelayCommand(StartScrape, CanStartScrape);
            StopCommand = new AsyncRelayCommand(StopScrape, CanStopScrape);
        }

        public string PluginKey => _plugin.PluginKey;
        public string ImportDescription => _plugin.ImportDescription;
        public string ImportSourceUrl => _plugin.ImportSourceUrl;

        string _steamId;
        public string SteamId
        {
            get => _steamId;
            set
            {
                SetProperty(ref _steamId, value);
                StartCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
            }
        }
        string _steamApiKey;
        public string SteamApiKey
        {
            get => _steamApiKey;
            set
            {
                SetProperty(ref _steamApiKey, value);
                StartCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
            }
        }
        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                SetProperty(ref _isProcessing, value);
                StopCommand.RaiseCanExecuteChanged();
            }
        }
        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        private ObservableCollection<AssetItem> _assets;
        public ObservableCollection<AssetItem> Assets
        {
            get => _assets;
            set => SetProperty(ref _assets, value);
        }

        bool CanStartScrape() => true;
        bool CanStopScrape() => IsProcessing;


        private async Task StopScrape()
        {
            if (!IsProcessing
                || _scrapingTask == null
                || _lastCancellationTokenSource == null)
            {
                System.Diagnostics.Debug.WriteLine($"Error cannot stop scraping {PluginKey}");
                return;
            }

            try
            {
                await _lastCancellationTokenSource.CancelAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error while cancelling scraping {PluginKey}: {ex.Message}");
            }
            StatusMessage = "Scraping cancelled by user.";
            _eventAggregator.Publish(new ServiceCommandExecutedEvent
            {
                ServiceName = PluginKey,
                CommandData = ServiceStatusEvents.Cancelled,
            });
        }

        Task<IEnumerable<OwnedAsset>> _scrapingTask;
        public async Task BeginScrapingInBrowserAsync(Microsoft.Web.WebView2.Wpf.WebView2 browser)
        {
            scrapingResults = new List<WebScrapingResult>();
            pageNumber = 1;

            _browser = browser;
            _browser.NavigationCompleted += Browser_NavigationCompleted;
            _browser.Source = new Uri(_assetImporter.ImporterAssetsUrl);          
        }

        public async Task EndScrapingInBrowserAsync()
        {
            _browser.NavigationCompleted -= Browser_NavigationCompleted;

            var assetItems = new List<AssetItem>();
            StringBuilder sb = new StringBuilder();
            foreach (var results in scrapingResults)
            {
                if (results.Success)
                {
                    assetItems.AddRange(results.OwnedAssets.Select(oa => new AssetItem(oa)));
                    sb.AppendLine($"Scraping from PageNumver {results.PageNumber} successfull.[{results.OwnedAssets.Count()}]");
                }
                else
                {
                    sb.AppendLine($"Scraping from PageNumber {results.PageNumber} failed. [{results.SourceUrl}]");
                }
            }
            Assets = new ObservableCollection<AssetItem>(assetItems.Distinct());
            sb.AppendLine($"Scraped {pageNumber} pages with {Assets.Count} unique assets.");

            StatusMessage = sb.ToString();
        }

        private async void Browser_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            string html = await _browser.ExecuteScriptAsync("document.documentElement.outerHTML;");
            html = System.Text.Json.JsonSerializer.Deserialize<string>(html);
            var scrapeResult = await _assetImporter.ImportAssetsFromHtmlSourceAsync(_browser.Source.ToString(), pageNumber, html);
            scrapingResults.Add(scrapeResult);
            if(string.IsNullOrEmpty(scrapeResult.NextPageUrl))
            {
                await EndScrapingInBrowserAsync();                
            }
            else
            {
                _browser.Source = new Uri(scrapeResult.NextPageUrl);
                ++pageNumber;
            }
        }

        List<WebScrapingResult> scrapingResults = new List<WebScrapingResult>();
        int pageNumber = 1;

        private async Task StartScrape()
        {
            _eventAggregator.Publish(new ServiceCommandExecutedEvent
            {
                ServiceName = PluginKey,
                CommandData = ServiceStatusEvents.Start
            });
            StatusMessage = "Scraping...";

            try
            {
                // Start progress animation
                IsProcessing = true;

                _lastCancellationTokenSource = new CancellationTokenSource();

                // Fetch data from Steam API
                _scrapingTask = _assetImporter.ImportAssetsAsync(_lastCancellationTokenSource.Token); //_steamService.GetSteamGamesAsync(SteamApiKey, SteamId, _lastCancellationTokenSource.Token);
                var games = await _scrapingTask;

                Assets = new ObservableCollection<AssetItem>(games.Select(g => new AssetItem(g)));
                StatusMessage = $"Loaded {Assets.Count} games successfully!";

                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = PluginKey,
                    CommandData = ServiceStatusEvents.Success
                });
                _eventAggregator.Publish(new ServiceDataChangedEvent
                {
                    ServiceName = PluginKey,
                    DataCount = Assets.Count()
                });
            }
            catch (OperationCanceledException ocex)
            {
                StatusMessage = $"Scraping was cancelled";
                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = PluginKey,
                    CommandData = ServiceStatusEvents.Cancelled
                });
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = PluginKey,
                    CommandData = ServiceStatusEvents.Failure
                });
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private void OpenLink(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
        }
    }
}
