using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.ViewModels;
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
        public ICommand OpenLinkCommand { get; }
        public IAsyncRelayCommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public ScrapeWizardViewModel(IEventAggregator eventAggregator, ISteamService steamService)
        {
            _eventAggregator = eventAggregator;
            _steamService = steamService;

            OpenLinkCommand = new RelayCommand<string>(OpenLink);
            StartCommand = new AsyncRelayCommand(StartScrape, CanStartScrape);
            StopCommand = new RelayCommand(StopScrape, CanStopScrape);
        }

        string _steamId;
        public string SteamId
        {
            get => _steamId;
            set
            {
                SetProperty(ref _steamId, value);
                OnPropertyChanged(nameof(CanStart));
                OnPropertyChanged(nameof(CanStop));
            }
        }
        string _steamApiKey;
        public string SteamApiKey
        {
            get => _steamApiKey;
            set
            {
                SetProperty(ref _steamApiKey, value);
                OnPropertyChanged(nameof(CanStart));
                OnPropertyChanged(nameof(CanStop));
            }
        }
        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set => SetProperty(ref _isProcessing, value);
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

        public bool CanStart
        {
            get => true;
            //get => !string.IsNullOrEmpty(SteamId) && !string.IsNullOrEmpty(SteamApiKey);
        }
        public bool CanStop
        {
            get => false;
        }

        bool CanStartScrape() => CanStart;
        bool CanStopScrape(object param) => CanStop;


        private void StopScrape(object obj)
        {
            throw new NotImplementedException();
        }

        private async Task StartScrape()
        {
            _eventAggregator.Publish(new ServiceCommandExecutedEvent
            {
                ServiceName = SteamScraperModule.ModuleName,
                CommandData = ServiceStatusEvents.Start
            });

            try
            {
                // Start progress animation
                IsProcessing = true;

                // Fetch data from Steam API
                var games = await _steamService.GetSteamGamesAsync(SteamApiKey, SteamId);


                Games = new ObservableCollection<SteamGame>(games);
                StatusMessage = $"Loaded {games.Count} games successfully!";

                _eventAggregator.Publish(new ServiceCommandExecutedEvent
                {
                    ServiceName = SteamScraperModule.ModuleName,
                    CommandData = ServiceStatusEvents.Success
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
