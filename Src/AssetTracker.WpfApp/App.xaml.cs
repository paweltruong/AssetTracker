using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Modules.Main.Extensions;
using AssetTracker.WpfApp.Modules.SteamScraper;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace AssetTracker.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        ServiceCollection _serviceCollection;
        ServiceProvider _serviceProvider;
        List<IScraperModule> _modules = new List<IScraperModule>();



        public App()
        {
            _serviceCollection = new ServiceCollection();
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
    }


}
