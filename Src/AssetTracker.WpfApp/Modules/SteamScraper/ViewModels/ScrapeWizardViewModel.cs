using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Modules.SteamScraper.Models;
using AssetTracker.WpfApp.Modules.SteamScraper.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace AssetTracker.WpfApp.Modules.SteamScraper.ViewModels
{
    public class ScrapeWizardViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ISteamService _steamService;
        private CancellationTokenSource _lastCancellationTokenSource;
        public ICommand OpenLinkCommand { get; }
        public IAsyncRelayCommand StartCommand { get; }
        public IAsyncRelayCommand StopCommand { get; }

        public ScrapeWizardViewModel(IEventAggregator eventAggregator, ISteamService steamService)
        {
            _eventAggregator = eventAggregator;
            _steamService = steamService;

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

        private ObservableCollection<SteamGame> _games;
        public ObservableCollection<SteamGame> Games
        {
            get => _games;
            set => SetProperty(ref _games, value);
        }

        bool CanStartScrape() => !string.IsNullOrEmpty(SteamId) && !string.IsNullOrEmpty(SteamApiKey);
        bool CanStopScrape() => IsProcessing;


        private async Task StopScrape()
        {
            if (!IsProcessing
                || _scrapingTask == null
                || _lastCancellationTokenSource == null)
            {
                System.Diagnostics.Debug.WriteLine($"Error cannot stop scraping {SteamScraperModule.ModuleName}");
                return;
            }

            try
            {
                await _lastCancellationTokenSource.CancelAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error while cancelling scraping {SteamScraperModule.ModuleName}: {ex.Message}");
            }
            StatusMessage = "Scraping cancelled by user.";
            _eventAggregator.Publish(new ServiceCommandExecutedEvent
            {
                ServiceName = SteamScraperModule.ModuleName,
                CommandData = ServiceStatusEvents.Cancelled,
            });
        }

        Task<List<SteamGame>> _scrapingTask;

        private async Task StartScrape()
        {
            _eventAggregator.Publish(new ServiceCommandExecutedEvent
            {
                ServiceName = SteamScraperModule.ModuleName,
                CommandData = ServiceStatusEvents.Start
            });
            StatusMessage = "Scraping...";

            try
            {
                // Start progress animation
                IsProcessing = true;

                _lastCancellationTokenSource = new CancellationTokenSource();

                // Fetch data from Steam API
                _scrapingTask = _steamService.GetSteamGamesAsync(SteamApiKey, SteamId, _lastCancellationTokenSource.Token);
                var games = await _scrapingTask;

                Games = new ObservableCollection<SteamGame>(games);
                StatusMessage = $"Loaded {games.Count} games successfully!";

                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = SteamScraperModule.ModuleName,
                    CommandData = ServiceStatusEvents.Success
                });
            }
            catch (OperationCanceledException ocex)
            {
                StatusMessage = $"Scraping was cancelled";
                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = SteamScraperModule.ModuleName,
                    CommandData = ServiceStatusEvents.Cancelled
                });
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error: {ex.Message}";
                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = SteamScraperModule.ModuleName,
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
