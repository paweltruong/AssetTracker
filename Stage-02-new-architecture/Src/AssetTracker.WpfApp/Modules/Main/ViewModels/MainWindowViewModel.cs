using AssetTracker.Core.Services;
using AssetTracker.Core.Services.AssetsComparer;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IViewModel
    {
        public const string MyAssetsViewKey = "MyAssets";
        public const string ImportsViewKey = "Imports";
        public const string CheckAssetsViewKey = "CheckAssets";
        public const string SettingsViewKey = "Settings";

        public const string MyAssetsButtonTextDefault = "My Assets";
        public const string ButtonTextDirtySuffix = "*";

        // Expose as instance properties for binding
        public string MyAssetsViewName => MyAssetsViewKey;
        public string ImportsViewName => ImportsViewKey;
        public string CheckAssetsViewName => CheckAssetsViewKey;
        public string SettingsViewName => SettingsViewKey;

        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAssetDatabase _assetDatabase;

        private string _myAssetsButtonText = MyAssetsButtonTextDefault;
        public string MyAssetsButtonText
        {
            get => _myAssetsButtonText;
            set => SetProperty(ref _myAssetsButtonText, value);
        }

        private ViewModelBase _currentView;
        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        private string _selectedView;
        public string SelectedView
        {
            get => _selectedView;
            set => SetProperty(ref _selectedView, value);
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

        public MainWindowViewModel(IServiceProvider serviceProvider,
            IEventAggregator eventAggregator,
            IAssetsComparer assetsComparer,
            IAssetDatabase assetDatabase)
        {
            _serviceProvider = serviceProvider;
            _eventAggregator = eventAggregator;
            _assetDatabase = assetDatabase;
            _assetDatabase.AssetDatabaseChanged += AssetDatabase_AssetDatabaseChanged;

            NavigateCommand = new RelayCommand<string>(ExecuteNavigateCommand);

            NavigateCommand.Execute("MyAssets");
        }

        private void AssetDatabase_AssetDatabaseChanged(object? sender, EventArgs e)
        {
            MyAssetsButtonText = MyAssetsButtonTextDefault + (_assetDatabase.IsDirty ? ButtonTextDirtySuffix : string.Empty);            
        }
    }
}
