using AssetTracker.Core.Models;
using AssetTracker.Core.Services;
using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.AssetsImporter.Definitions;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using Microsoft.Web.WebView2.Core;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class DefaultBrowserAssetsImporterViewModel : ViewModelBase
    {
        private IAssetsImporterPlugin _plugin;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAssetsImporter _assetImporter;
        private readonly IAssetDatabase _assetDatabase;

        private CancellationTokenSource _lastCancellationTokenSource;

        List<WebScrapingResult> scrapingResults = new List<WebScrapingResult>();
        int pageNumber = 1;


        public ICommand OpenLinkCommand { get; }
        public IAsyncRelayCommand StartCommand { get; }
        public IAsyncRelayCommand StopCommand { get; }

        Microsoft.Web.WebView2.Wpf.WebView2 _browser;

        public DefaultBrowserAssetsImporterViewModel(IEventAggregator eventAggregator,
            IAssetsImporterPlugin plugin,
            IAssetsImporter assetImporter,
            IAssetDatabase assetDatabase)
        {
            _eventAggregator = eventAggregator;
            _plugin = plugin;
            _assetImporter = assetImporter;
            _assetDatabase = assetDatabase;

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

        private bool _isBrowserLoading;
        public bool IsBrowserLoading
        {
            get => _isBrowserLoading;
            set
            {
                SetProperty(ref _isBrowserLoading, value);
                OnPropertyChanged(nameof(IsBusy));
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsBusy => IsBrowserLoading || IsProcessing;

        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                SetProperty(ref _isProcessing, value);
                OnPropertyChanged(nameof(IsBusy));
                StopCommand.RaiseCanExecuteChanged();
                StartCommand.RaiseCanExecuteChanged();
            }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        private ObservableCollection<OwnedAsset> _importedAssets;
        public ObservableCollection<OwnedAsset> ImportedAssets
        {
            get => _importedAssets;
            set => SetProperty(ref _importedAssets, value);
        }

        bool CanStartScrape() => !IsBusy;
        bool CanStopScrape() => IsProcessing;

        public void SetupBrowser(Microsoft.Web.WebView2.Wpf.WebView2 browser)
        {
            if (_browser != null) return;
            _browser = browser;
            _browser.Source = new Uri(_plugin.ImportSourceUrl);
            _browser.NavigationStarting += _browser_NavigationStarting;// += Browser_ContentLoading;
            _browser.NavigationCompleted += _browser_NavigationCompleted;
        }

        private void _browser_NavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            IsBrowserLoading = true;
        }
        private async void _browser_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            IsBrowserLoading = false;
        }

        private void Browser_ContentLoading(object? sender, CoreWebView2ContentLoadingEventArgs e)
        {
            IsBrowserLoading = true;
        }

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
        public async Task BeginScrapingInBrowserAsync(CancellationToken cancellationToken = default)
        {
            scrapingResults = new List<WebScrapingResult>();
            pageNumber = 1;

            _browser.NavigationCompleted += Browser_NavigationCompleted;
            _browser.Source = new Uri(_assetImporter.ImporterAssetsUrl);
        }

        public async Task EndScrapingInBrowserAsync(bool success)
        {
            _browser.NavigationCompleted -= Browser_NavigationCompleted;

            if (success)
            {
                var assetItems = new List<OwnedAsset>();
                StringBuilder sb = new StringBuilder();
                foreach (var results in scrapingResults)
                {
                    if (results.Success)
                    {
                        assetItems.AddRange(results.OwnedAssets);
                        sb.AppendLine($"Scraping from PageNumver {results.PageNumber} successfull.[{results.OwnedAssets.Count()}]");
                    }
                    else
                    {
                        sb.AppendLine($"Scraping from PageNumber {results.PageNumber} failed. [{results.SourceUrl}]");
                    }
                }
                ImportedAssets = new ObservableCollection<OwnedAsset>(assetItems.Distinct());
                await _assetDatabase.AddAssetsAsync(ImportedAssets);
                sb.AppendLine($"Scraped {pageNumber} pages with {ImportedAssets.Count} unique assets.");


                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = PluginKey,
                    CommandData = ServiceStatusEvents.Success
                });
                _eventAggregator.Publish(new ServiceDataChangedEvent
                {
                    ServiceName = PluginKey,
                    DataCount = ImportedAssets.Count()
                });

                StatusMessage = sb.ToString();
            }

            IsProcessing = false;
        }

        private async void Browser_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (_lastCancellationTokenSource.IsCancellationRequested)
            {
                await EndScrapingInBrowserAsync(false);
                return;
            }

            string html = await _browser.ExecuteScriptAsync("document.documentElement.outerHTML;");
            html = System.Text.Json.JsonSerializer.Deserialize<string>(html);

            try
            {
                var scrapeResult = await _assetImporter.ImportAssetsFromHtmlSourceAsync(_browser.Source.ToString(), pageNumber, html, _lastCancellationTokenSource.Token);
                scrapingResults.Add(scrapeResult);
                if (string.IsNullOrEmpty(scrapeResult.NextPageUrl))
                {
                    await EndScrapingInBrowserAsync(true);
                }
                else
                {
                    _browser.Source = new Uri(scrapeResult.NextPageUrl);
                    ++pageNumber;
                }
            }
            catch (OperationCanceledException ocex)
            {
                StatusMessage = $"Scraping was cancelled";
                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = PluginKey,
                    CommandData = ServiceStatusEvents.Cancelled
                });
                await EndScrapingInBrowserAsync(false);
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = PluginKey,
                    CommandData = ServiceStatusEvents.Failure
                });
                await EndScrapingInBrowserAsync(false);
            }
        }

        private async Task StartScrape()
        {
            if (_browser == null)
            {
                StatusMessage = $"Error browser is not initialized";
                return;
            }

            _eventAggregator.Publish(new ServiceCommandExecutedEvent
            {
                ServiceName = PluginKey,
                CommandData = ServiceStatusEvents.Start
            });
            StatusMessage = "Scraping, Please dont click within the browser it could affect the scraping...";

            // Start progress animation
            IsProcessing = true;

            _lastCancellationTokenSource = new CancellationTokenSource();
            try
            {
                await BeginScrapingInBrowserAsync(_lastCancellationTokenSource.Token);
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
