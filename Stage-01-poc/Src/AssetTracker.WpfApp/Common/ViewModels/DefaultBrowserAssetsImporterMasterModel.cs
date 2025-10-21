using AssetTracker.Core.Services.AssetsImporter;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class DefaultBrowserAssetsImporterMasterModel : IScraperServiceMasterModel
    {
        public string ModuleName { get; private set; }

        public IScraperServiceListItemView? ListItemView { get; private set; }

        public IScraperServiceMainView? DefaultMainView { get; private set; }

        public IScraperServiceMainView? ImportAssetsView { get; private set; }

        public DefaultBrowserAssetsImporterMasterModel(IServiceProvider serviceProvider, IAssetsImporterPlugin plugin)
        {
            ModuleName = plugin.PluginKey;

            var listItemView = new ScraperServiceListItemView();
            var listItemViewModel = serviceProvider.GetRequiredService<DefaultBrowserAssetsImporterListItemViewModel>();
            listItemViewModel.SetListItemProperties(plugin, this);
            listItemView.DataContext = listItemViewModel;
            ListItemView = listItemView;

            var factory = serviceProvider.GetRequiredService<IImportAssetsViewModelFactory>();
            var importAssetsViewModel = factory.Create(plugin);   
            var importAssetsView = new DefaultBrowserAssetsImporterView();
            importAssetsView.DataContext = importAssetsViewModel;
            ImportAssetsView = importAssetsView;
        }
    }
}
