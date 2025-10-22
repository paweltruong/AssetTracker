using AssetTracker.Core.Models;
using AssetTracker.Core.Services;
using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Modules.SteamScraper;
using System.Collections.ObjectModel;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class DefaultHttpClientAssetsImporterViewModel : AssetImporterPluginViewModelBase
    {
        public DefaultHttpClientAssetsImporterViewModel(IEventAggregator eventAggregator,
            IAssetsImporterPlugin plugin,
            IAssetsImporter assetImporter,
            IAssetDatabase assetDatabase) : base(eventAggregator, plugin, assetImporter, assetDatabase)
        {
            InitializeCallParameters();
        }

        Task<IEnumerable<OwnedAsset>> _scrapingTask;
        public override bool IsBusy => IsProcessing;

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

        private ObservableCollection<OwnedAsset> _games;
        public ObservableCollection<OwnedAsset> Games
        {
            get => _games;
            set => SetProperty(ref _games, value);
        }
        private Dictionary<string, string> _parameterValues = new Dictionary<string, string>();
        public ObservableCollection<CallParameter> CallParameters { get; } = new ObservableCollection<CallParameter>();

        private void InitializeCallParameters()
        {
            foreach (var param in _plugin.UseHttpClientCallParams)
            {
                CallParameters.Add(new CallParameter
                {
                    Key = param.Key,
                    Label =param.Key,
                    Description = param.Value
                });
            }
        }

        // Handle password changes
        public void OnParameterPasswordChanged(string key, string password)
        {
            // Store the password securely in your ViewModel
            // You might want to use a dictionary to store parameter values
            if (_parameterValues.ContainsKey(key))
            {
                _parameterValues[key] = password;
            }
            else
            {
                _parameterValues.Add(key, password);
            }
            StartCommand.RaiseCanExecuteChanged();
        }

        protected override bool CanStartScrape() => _parameterValues.Count == _plugin.UseHttpClientCallParams.Count
            && !_parameterValues.Values.Any(v => string.IsNullOrEmpty(v));

        protected override bool CanStopScrape() => IsProcessing;

        protected override async Task StopScrape()
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

        protected override async Task StartScrape()
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
                _scrapingTask = _assetImporter.ImportAssetsFromHttpClientAsync(_plugin.ImportApiCallMethod, _plugin.ImportApiUrl, _parameterValues, _lastCancellationTokenSource.Token);
                var games = await _scrapingTask;

                await _assetDatabase.AddAssetsAsync(games);
                Games = new ObservableCollection<OwnedAsset>(games);
                StatusMessage = $"Loaded {games.Count()} games successfully!";

                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = PluginKey,
                    CommandData = ServiceStatusEvents.Success
                });
                _eventAggregator.Publish(new ServiceDataChangedEvent
                {
                    ServiceName = PluginKey,
                    DataCount = Games.Count()
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
    }
}
