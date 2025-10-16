using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Modules.Main.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.WpfApp.Modules.Main.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<IEventAggregator, EventAggregator>();
        }
    }
}
