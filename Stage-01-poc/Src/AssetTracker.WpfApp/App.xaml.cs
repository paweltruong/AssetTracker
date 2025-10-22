using AssetTracker.Application;
using AssetTracker.AssetsImporter.Steam;
using AssetTracker.AssetsImporter.SyntyStore;
using AssetTracker.AssetsResolver.HumbleBundle;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Common.Views;
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

        public static List<IScraperModule> Modules { get; private set; }
        public static List<IPlugin> Plugins { get; private set; }
        Func<IServiceProvider, object?, IAssetsImporterPlugin> implementationFactory;

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

            //_modules.Add(new SteamScraperModule());
            //_modules.Add(new HumbleBundleModule());
            //foreach (var module in _modules)
            //{
            //    module.ConfigureModule(_serviceCollection);
            //}
            Modules = _modules;

            _plugins.Add(new SteamAssetsImporterPlugin());
            _plugins.Add(new SyntyStoreAssetsImporterPlugin());
            _plugins.Add(new HumbleBundleAssetsResolverPlugin());
            foreach (var plugin in _plugins)
            {
                plugin.ConfigureServices(_serviceCollection);
                if(plugin is IAssetsImporterPlugin assetsImporterPlugin )
                {
                    if (assetsImporterPlugin.UseDefaultBrowserLayout)
                    {
                        _serviceCollection.AddKeyedSingleton<AssetsDataView>(plugin.PluginKey);
                    }
                    else if(assetsImporterPlugin.UseDefaultHttpClientLayout)
                    {
                        _serviceCollection.AddKeyedSingleton<AssetsDataView>(plugin.PluginKey);
                    }
                }
            }
            Plugins = _plugins;

            _serviceProvider = _serviceCollection.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }

}
