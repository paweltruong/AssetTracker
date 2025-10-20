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
        private ILoggerFactory _loggerFactory;

        public App()
        {
            // Configure logging first
            ConfigureLogging();

            _serviceCollection = new ServiceCollection();

            // Add logging services first
            _serviceCollection.AddSingleton(_loggerFactory);

            // Configure your existing services
            _serviceCollection.ConfigureServices();

            _modules.Add(new SteamScraperModule());

            foreach (var module in _modules)
            {
                module.ConfigureModule(_serviceCollection);
            }

            _serviceProvider = _serviceCollection.BuildServiceProvider();

            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.ViewModel.LoadScraperServices(_serviceProvider, _modules);
            mainWindow.Show();
        }

        private void ConfigureLogging()
        {
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.ClearProviders(); 
                builder.AddDebug();        
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Trace);
                // You can add more providers here as needed
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _loggerFactory?.Dispose();
            _serviceProvider?.Dispose();
            base.OnExit(e);
        }
    }

}
