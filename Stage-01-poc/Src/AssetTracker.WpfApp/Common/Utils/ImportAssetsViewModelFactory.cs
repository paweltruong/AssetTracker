using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.WpfApp.Common.Utils
{
    public interface IImportAssetsViewModelFactory
    {
        DefaultBrowserAssetsImporterViewModel Create(IAssetsImporterPlugin plugin);
    }

    public class ImportAssetsViewModelFactory : IImportAssetsViewModelFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ImportAssetsViewModelFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public DefaultBrowserAssetsImporterViewModel Create(IAssetsImporterPlugin plugin)
        {
            return new DefaultBrowserAssetsImporterViewModel(
                _serviceProvider.GetRequiredService<IEventAggregator>(),
                plugin,
                _serviceProvider.GetRequiredKeyedService<IAssetsImporter>(plugin.PluginKey)
            );
        }
    }
}
