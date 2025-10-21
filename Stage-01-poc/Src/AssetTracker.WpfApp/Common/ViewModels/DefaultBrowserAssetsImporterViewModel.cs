using AssetTracker.Core.Models;
using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class DefaultBrowserAssetsImporterViewModel : ViewModelBase
    {
        private string _pluginKey;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAssetsImporter _assetImporter;

        private CancellationTokenSource _lastCancellationTokenSource;
        public ICommand OpenLinkCommand { get; }
        public IAsyncRelayCommand StartCommand { get; }
        public IAsyncRelayCommand StopCommand { get; }

        public DefaultBrowserAssetsImporterViewModel(IEventAggregator eventAggregator, IPlugin plugin, IAssetsImporter assetImporter)
        {
            _eventAggregator = eventAggregator;
            _pluginKey = plugin.PluginKey;
            _assetImporter = assetImporter;

            OpenLinkCommand = new RelayCommand<string>(OpenLink);
            StartCommand = new AsyncRelayCommand(StartScrape, CanStartScrape);
            StopCommand = new AsyncRelayCommand(StopScrape, CanStopScrape);
        }

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

        bool CanStartScrape() => !string.IsNullOrEmpty(SteamId) && !string.IsNullOrEmpty(SteamApiKey);
        bool CanStopScrape() => IsProcessing;


        private async Task StopScrape()
        {
            if (!IsProcessing
                || _scrapingTask == null
                || _lastCancellationTokenSource == null)
            {
                System.Diagnostics.Debug.WriteLine($"Error cannot stop scraping {_pluginKey}");
                return;
            }

            try
            {
                await _lastCancellationTokenSource.CancelAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error while cancelling scraping {_pluginKey}: {ex.Message}");
            }
            StatusMessage = "Scraping cancelled by user.";
            _eventAggregator.Publish(new ServiceCommandExecutedEvent
            {
                ServiceName = _pluginKey,
                CommandData = ServiceStatusEvents.Cancelled,
            });
        }

        Task<IEnumerable<OwnedAsset>> _scrapingTask;

        private async Task StartScrape()
        {
            _eventAggregator.Publish(new ServiceCommandExecutedEvent
            {
                ServiceName = _pluginKey,
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
                    ServiceName = _pluginKey,
                    CommandData = ServiceStatusEvents.Success
                });
                _eventAggregator.Publish(new ServiceDataChangedEvent
                {
                    ServiceName = _pluginKey,
                    DataCount = Assets.Count()
                });
            }
            catch (OperationCanceledException ocex)
            {
                StatusMessage = $"Scraping was cancelled";
                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = _pluginKey,
                    CommandData = ServiceStatusEvents.Cancelled
                });
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = _pluginKey,
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
