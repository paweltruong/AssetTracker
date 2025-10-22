using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;
using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.SteamScraper.Views;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class DefaultBrowserAssetsImporterListItemViewModel : ScraperServiceListItemViewModel<ScraperServiceDataModel>
    {
        IAssetsImporterPlugin _plugin;
        IScraperServiceMasterModel _masterModel;

        public DefaultBrowserAssetsImporterListItemViewModel(IEventAggregator eventAggregator) : base(eventAggregator)
        {
            Model.Title = "Plugin";
            Model.Description = "Get owned assets";
            Model.IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9d/Emblem-unreadable.svg/40px-Emblem-unreadable.svg.png";
            Model.Status = ScraperServiceStatus.NotLoaded;
        }

        public string PluginKey => _plugin?.PluginKey ?? string.Empty;

        public void SetListItemProperties(IAssetsImporterPlugin plugin, IScraperServiceMasterModel masterModel)
        {
            _plugin = plugin;
            _masterModel = masterModel;

            Model.Title = plugin.DisplayName;
            Model.Description = plugin.Description;
            Model.IconUrl = plugin.IconUrl;
            OnPropertyChanged(nameof(Model));
        }

        // Command methods
        protected override void ExecuteConfigureServiceCommand(object parameter)
        {
            _eventAggregator.Publish(new ChangeMainViewEvent(_plugin, _masterModel.ImportAssetsView));
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
            _eventAggregator.Publish(new ChangeMainViewEvent(_plugin,  typeof(AssetsDataView)));
        }

        protected override void OnServiceCommandExecuted(ServiceCommandExecutedEvent eventData)
        {
            base.OnServiceCommandExecuted(eventData);

            // Handle the event at higher level
            if (eventData != null
                && eventData.ServiceName.Equals(PluginKey)
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
                && eventData.ServiceName.Equals(PluginKey))
            {
                Model.ViewDataButtonText = eventData.DataCount > 0 ? 
                    string.Format(ScraperServiceDataModel.ViewDataButtonTextFormatted, eventData.DataCount)
                    : ScraperServiceDataModel.ViewDataButtonTextDefault;
                OnPropertyChanged(nameof(Model));
            }
        }
    }
}
