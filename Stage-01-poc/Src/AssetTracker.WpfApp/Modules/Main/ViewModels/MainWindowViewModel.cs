using AssetTracker.Core.Models.Enums;
using AssetTracker.Core.Services.AssetsComparer;
using AssetTracker.Core.Services.AssetsResolver;
using AssetTracker.Core.Services.AssetsResolver.Definitions;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.Main.Views;
using AssetTracker.WpfApp.Modules.SteamScraper;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IViewModel
    {
        public const string MyAssetsViewKey = "MyAssets";
        public const string ImportsViewKey = "Imports";
        public const string CheckAssetsViewKey = "CheckAssets";
        public const string SettingsViewKey = "Settings";

        // Expose as instance properties for binding
        public string MyAssetsViewName => MyAssetsViewKey;
        public string ImportsViewName => ImportsViewKey;
        public string CheckAssetsViewName => CheckAssetsViewKey;
        public string SettingsViewName => SettingsViewKey;

        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAggregator _eventAggregator;


        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        private string _selectedView;
        public string SelectedView
        {
            get => _selectedView;
            set
            {
                _selectedView = value;
                OnPropertyChanged();
            }
        }
        public IRelayCommand NavigateCommand { get; }

        private void ExecuteNavigateCommand(string viewName)
        {
            SelectedView = viewName;

            CurrentView = viewName switch
            {
                CheckAssetsViewKey => _serviceProvider.GetRequiredService<CheckAssetsViewModel>(),
                SettingsViewKey => _serviceProvider.GetRequiredService<SettingsViewModel>(),
                ImportsViewKey => _serviceProvider.GetRequiredService<ImportsViewModel>(),
                _ => _serviceProvider.GetRequiredService<MyAssetsViewModel>(),
            };
        }

        public MainWindowViewModel(IServiceProvider serviceProvider, IEventAggregator eventAggregator, IAssetsComparer assetsComparer)
        {
            _serviceProvider = serviceProvider;
            _eventAggregator = eventAggregator;

            NavigateCommand = new RelayCommand<string>(ExecuteNavigateCommand);           

            NavigateCommand.Execute("MyAssets");
        }
    }
}
