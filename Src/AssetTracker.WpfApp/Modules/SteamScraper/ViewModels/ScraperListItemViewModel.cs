using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Modules.SteamScraper.Views;
using System.Windows;

namespace AssetTracker.WpfApp.Modules.SteamScraper.ViewModels
{
    public class ScraperListItemViewModel : ScraperServiceViewModel<ScraperServiceDataModel>
    {

        public ScraperListItemViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            Model.Title = "Steam";
            Model.Description = "Get owned game list";
            Model.IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/512px-Steam_icon_logo.svg.png";
            Model.Status = ScraperServiceStatus.NotLoaded;
        }
        // Command methods
        protected override void ConfigureService(object parameter)
        {            
            _eventAggregator.Publish(new ChangeMainViewEvent
            {
                ServiceName = SteamScraperModule.ModuleName,
                MainView = typeof(ScrapeWizardView)
            });
        }
        protected override bool CanStopService(object parameter) => false;
        protected override void StopService(object parameter)
        {
            // Your business logic here

            // Update UI properties
            OnPropertyChanged(nameof(Model));

            // Refresh command states
            ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)StopCommand).RaiseCanExecuteChanged();

            MessageBox.Show("Scraper service stopped!");
        }
        protected override bool CanStartService(object parameter) => true;
        protected override void StartService(object parameter)
        {
            // Your business logic here
            _model.Status = ScraperServiceStatus.Running;

            // Update UI properties
            OnPropertyChanged(nameof(Model));

            // Refresh command states
            ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)StopCommand).RaiseCanExecuteChanged();

            MessageBox.Show("Scraper service started!");
        }

        protected override void OnServiceCommandExecuted(ServiceCommandExecutedEvent eventData)
        {
            // Handle the event at higher level
            if(eventData != null 
                && eventData.ServiceName.Equals(SteamScraperModule.ModuleName)
                && eventData.CommandData is ServiceStatusEvents)
            {
                switch (eventData.CommandData)
                {
                    case ServiceStatusEvents.Start:
                        Model.Status = ScraperServiceStatus.Running;
                        break;
                    //case ServiceCommands.Stop:
                    //    StopService(null);
                    //    break;
                    //case ServiceCommands.Configure:
                    //    ConfigureService(null);
                    //    break;
                    default:
                        break;
                }
            }
        }
    }
}
