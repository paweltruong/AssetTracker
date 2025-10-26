using AssetTracker.Core.Services;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;
using AssetTracker.WpfApp.Common.Views;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class DefaultBrowserAssetsImporterListItemViewModel : ScraperServiceListItemViewModel<ScraperServiceDataModel>
    {
        IAssetsImporterPlugin _plugin;
        IScraperServiceMasterModel _masterModel;

        public DefaultBrowserAssetsImporterListItemViewModel(
            IEventAggregator eventAggregator,
            IAssetDatabase assetDatabase)
            : base(eventAggregator, assetDatabase)
        {
            Model.Title = "Plugin";
            Model.Description = "Get owned assets";
            Model.IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9d/Emblem-unreadable.svg/40px-Emblem-unreadable.svg.png";
            Model.Status = ScraperServiceStatus.NotLoaded;
        }

        public string PluginKey => _plugin?.PluginKey ?? string.Empty;
        public string PluginMarketplaceKey => _plugin.MarketplaceKey ?? string.Empty;

        public void SetListItemProperties(IAssetsImporterPlugin plugin, IScraperServiceMasterModel masterModel)
        {
            _plugin = plugin;
            _masterModel = masterModel;

            Model.Title = plugin.DisplayName;
            Model.Description = plugin.Description;
            Model.IconUrl = plugin.IconUrl;

            UpdateViewButtonText();
            UpdateStatusWhenLoaded();
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
            _eventAggregator.Publish(new ChangeMainViewEvent(_plugin, typeof(AssetsDataView)));
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
                        Model.DateImported = eventData.EventDate;
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
                UpdateViewButtonText(eventData.DataCount);
            }
        }


        protected override void AssetDatabase_AssetDatabaseLoaded(object? sender, EventArgs e)
        {
            base.AssetDatabase_AssetDatabaseLoaded(sender, e);

            UpdateViewButtonText();
            UpdateStatusWhenLoaded();            
        }

        private void UpdateStatusWhenLoaded()
        {
            if (_assetDatabase.HasAnyAssetsFromMarketplace(PluginMarketplaceKey))
            {
                Model.Status = ScraperServiceStatus.DataLoaded;
                Model.DateImported = _assetDatabase.GetImportDate(PluginMarketplaceKey);
                OnPropertyChanged(nameof(Model));
            }
        }

        void UpdateViewButtonText(int? count = null)
        {
            if (!count.HasValue)
            {
                var assetsForPlugin = _assetDatabase.GetAssetsForMarketplace(PluginMarketplaceKey);
                count = assetsForPlugin.Count();
            }
            Model.ViewDataButtonText = count > 0 ?
                    string.Format(ScraperServiceDataModel.ViewDataButtonTextFormatted, count)
                    : ScraperServiceDataModel.ViewDataButtonTextDefault;

            OnPropertyChanged(nameof(Model));
        }
    }
}
