using AssetTracker.Application.Services;
using AssetTracker.Core.Services.AssetsComparer;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Services;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.ViewModels;
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

            services.AddSingleton<MyAssetsViewModel>();
            services.AddSingleton<ImportsViewModel>();
            services.AddSingleton<CheckAssetsViewModel>();
            services.AddSingleton<SettingsViewModel>();

            services.AddSingleton<IEventAggregator, EventAggregator>();
            services.AddTransient<IMyHttpClient, MyHttpClient>();
            services.AddSingleton<IAssetsComparer, DefaultAssetsComparer>();
            services.AddSingleton<IImportAssetsViewModelFactory, ImportAssetsViewModelFactory>();
            services.AddSingleton<DefaultBrowserAssetsImporterListItemViewModel>();
        }
    }
}
