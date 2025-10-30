using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Commands;
using System.ComponentModel.Design;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public abstract class AssetImporterPluginViewModelBase : ViewModelBase
    {
        protected readonly IAssetsImporterPlugin _plugin;
        protected readonly IEventAggregator _eventAggregator;
        protected readonly IAssetsImporter _assetImporter;
        protected readonly IAssetDatabase _assetDatabase;

        protected CancellationTokenSource _lastCancellationTokenSource;

        public string PluginKey => _plugin.PluginKey;
        public string ImportDescription => _plugin.ImportDescription;
        public abstract bool IsBusy { get; }

        public AssetImporterPluginViewModelBase(IEventAggregator eventAggregator,
            IAssetsImporterPlugin plugin,
            IAssetsImporter assetImporter,
            IAssetDatabase assetDatabase)
        {
            _plugin = plugin;
            _eventAggregator = eventAggregator;
            _assetImporter = assetImporter;
            _assetDatabase = assetDatabase;

            StartCommand = new AsyncRelayCommand(StartScrape, CanStartScrape);
            StopCommand = new AsyncRelayCommand(StopScrape, CanStopScrape);
        }


        protected abstract bool CanStartScrape();
        protected abstract bool CanStopScrape();

        protected abstract Task StopScrape();
        protected abstract Task StartScrape();

        public IAsyncRelayCommand StartCommand { get; }
        public IAsyncRelayCommand StopCommand { get; }

    }
}
