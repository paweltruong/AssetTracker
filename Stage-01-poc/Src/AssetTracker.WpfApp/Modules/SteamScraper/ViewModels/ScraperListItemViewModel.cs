using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Modules.SteamScraper.Views;

namespace AssetTracker.WpfApp.Modules.SteamScraper.ViewModels
{
    public class ScraperListItemViewModel : ScraperServiceListItemViewModel<ScraperServiceDataModel>
    {
        public ScraperListItemViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            Model.Title = "Steam";
            Model.Description = "Get owned game list";
            Model.IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/512px-Steam_icon_logo.svg.png";
            Model.Status = ScraperServiceStatus.NotLoaded;
        }

        // Command methods
        protected override void ExecuteConfigureServiceCommand(object parameter)
        {
            _eventAggregator.Publish(new ChangeMainViewEvent(SteamScraperModule.ModuleName, typeof(ScrapeWizardView)));
        }
        protected override bool CanExecuteOpenFileCommand(object parameter) => false;
        protected override void ExecuteOpenFileCommand(object parameter)
        {
            throw new NotImplementedException();
        }
        protected override bool CanExecuteSaveFileCommand(object parameter) => true;
        protected override void ExecuteSaveFileCommand(object parameter)
        {
           throw new NotImplementedException();
        }

        protected override bool CanExecuteViewDataCommand(object parameter)
        {
            //TODO:
            return true;
        }

        protected override void ExecuteViewDataCommand(object parameter)
        {
            _eventAggregator.Publish(new ChangeMainViewEvent(SteamScraperModule.ModuleName, typeof(DataView)));
        }

        protected override void OnServiceCommandExecuted(ServiceCommandExecutedEvent eventData)
        {
            base.OnServiceCommandExecuted(eventData);

            // Handle the event at higher level
            if (eventData != null
                && eventData.ServiceName.Equals(SteamScraperModule.ModuleName)
                && eventData.CommandData is ServiceStatusEvents)
            {
                switch (eventData.CommandData)
                {
                    case ServiceStatusEvents.Start:
                        Model.Status = ScraperServiceStatus.Running;
                        OnPropertyChanged(nameof(Model));
                        break;
                    case ServiceStatusEvents.Success:
                        Model.Status = ScraperServiceStatus.DataLoaded;
                        OnPropertyChanged(nameof(Model));
                        break;
                    case ServiceStatusEvents.Failure:
                        Model.Status = ScraperServiceStatus.Failed;
                        OnPropertyChanged(nameof(Model));
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

        protected override void OnServiceDataChanged(ServiceDataChangedEvent eventData)
        {
            base.OnServiceDataChanged(eventData);

            if (eventData != null
                && eventData.ServiceName.Equals(SteamScraperModule.ModuleName))
            {
                Model.ViewDataButtonText = eventData.DataCount > 0 ? 
                    string.Format(ScraperServiceDataModel.ViewDataButtonTextFormatted, eventData.DataCount)
                    : ScraperServiceDataModel.ViewDataButtonTextDefault;
                OnPropertyChanged(nameof(Model));
            }
        }
    }
}
