using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.ViewModels;
using System.Diagnostics;
using System.Windows.Input;

namespace AssetTracker.WpfApp.Modules.SteamScraper.ViewModels
{
    public class ScrapeWizardViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        public ICommand OpenLinkCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public ScrapeWizardViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            OpenLinkCommand = new RelayCommand<string>(OpenLink);
            StartCommand = new RelayCommand(StartScrape);
            StopCommand = new RelayCommand(StopScrape);
        }

        string _steamId;
        public string SteamId
        {
            get => _steamId;
            set => SetProperty(ref _steamId, value);
        }
        string _steamApiKey;
        public string SteamApiKey
        {
            get => _steamApiKey;
            set => SetProperty(ref _steamApiKey, value);
        }

        private void StopScrape(object obj)
        {
            throw new NotImplementedException();
        }

        private void StartScrape(object obj)
        {
            _eventAggregator.Publish(new ServiceCommandExecutedEvent
            {
                ServiceName = SteamScraperModule.ModuleName,
                CommandData = ServiceStatusEvents.Start
            });
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
