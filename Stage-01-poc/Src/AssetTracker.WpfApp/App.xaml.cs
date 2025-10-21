using AssetTracker.Application;
using AssetTracker.AssetsImporter.SyntyStore;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Modules.Main.Extensions;
using AssetTracker.WpfApp.Modules.SteamScraper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace AssetTracker.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private ServiceCollection _serviceCollection;
        private ServiceProvider _serviceProvider;
        private List<IScraperModule> _modules = new List<IScraperModule>();
        private List<IPlugin> _plugins = new List<IPlugin>();

        public App()
        {
            _serviceCollection = new ServiceCollection();

            // Add logging services first
            _serviceCollection.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddDebug();
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Trace);
            });

            // Configure your existing services
            _serviceCollection.ConfigureServices();
            ApplicationIocConfig.RegisterServices(_serviceCollection);

            _modules.Add(new SteamScraperModule());
            _modules.Add(new HumbleBundleModule());
            foreach (var module in _modules)
            {
                module.ConfigureModule(_serviceCollection);
            }

            _plugins.Add(new SyntyStoreAssetsImporterPlugin());
            foreach (var plugin in _plugins)
            {
                plugin.ConfigureServices(_serviceCollection);
            }

            _serviceProvider = _serviceCollection.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.ViewModel.LoadAssetsImporterPlugins(_serviceProvider, _modules, _plugins);
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }

}
