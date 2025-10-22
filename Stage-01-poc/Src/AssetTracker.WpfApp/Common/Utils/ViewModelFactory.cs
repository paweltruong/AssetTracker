using AssetTracker.Core.Services;
using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.WpfApp.Common.Utils
{
    public interface IViewModelFactory
    {
        DefaultBrowserAssetsImporterViewModel CreateDefaultBrowserAssetsImporterViewModel(IAssetsImporterPlugin plugin);
        DefaultHttpClientAssetsImporterViewModel CreateDefaultHttpClientAssetsImporterViewModel(IAssetsImporterPlugin plugin);

        AssetsDataViewModel CreateAssetsDataViewModel(IPlugin plugin);
    }

    public class ViewModelFactory : IViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public DefaultBrowserAssetsImporterViewModel CreateDefaultBrowserAssetsImporterViewModel(IAssetsImporterPlugin plugin)
        {
            return new DefaultBrowserAssetsImporterViewModel(
                _serviceProvider.GetRequiredService<IEventAggregator>(),
                plugin,
                _serviceProvider.GetRequiredKeyedService<IAssetsImporter>(plugin.PluginKey),
                _serviceProvider.GetRequiredService<IAssetDatabase>()
            );
        }
        public AssetsDataViewModel CreateAssetsDataViewModel(IPlugin plugin)
        {
            return new AssetsDataViewModel(plugin,
                _serviceProvider.GetRequiredService<IAssetDatabase>()
            );
        }

        public DefaultHttpClientAssetsImporterViewModel CreateDefaultHttpClientAssetsImporterViewModel(IAssetsImporterPlugin plugin)
        {
            return new DefaultHttpClientAssetsImporterViewModel(
                _serviceProvider.GetRequiredService<IEventAggregator>(),
                plugin,
                _serviceProvider.GetRequiredKeyedService<IAssetsImporter>(plugin.PluginKey),
                _serviceProvider.GetRequiredService<IAssetDatabase>()
            );
        }
    }
}
