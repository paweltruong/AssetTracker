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
    public partial class App : Application
    {
        private ServiceCollection _serviceCollection;
        private ServiceProvider _serviceProvider;
        private List<IScraperModule> _modules = new List<IScraperModule>();

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

            _modules.Add(new SteamScraperModule());
            _modules.Add(new HumbleBundleModule());

            foreach (var module in _modules)
            {
                module.ConfigureModule(_serviceCollection);
            }

            _serviceProvider = _serviceCollection.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.ViewModel.LoadScraperServices(_serviceProvider, _modules);
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }

}
