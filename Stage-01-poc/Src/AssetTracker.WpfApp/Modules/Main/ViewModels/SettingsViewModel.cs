using AssetTracker.Application.Services;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.ViewModels;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ICacheManager _cacheManager;
        public SettingsViewModel(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;

            ConfigPath = _cacheManager.CacheFolderPath;

            OpenLocationCommand = new RelayCommand<string>(ExecuteOpenLocationCommand, CanExecuteOpenLocationCommand);
        }

        string _configPath = string.Empty;
        public string ConfigPath
        {
            get=> _configPath;
            set => SetProperty(ref _configPath, value);
        }

        public IRelayCommand OpenLocationCommand { get; }


        private bool CanExecuteOpenLocationCommand(string path)
        {
            return WpfHelpers.CanOpenPath(path);
        }

        private void ExecuteOpenLocationCommand(string path)
        {
            WpfHelpers.OpenPath(path);
        }
    }
}
