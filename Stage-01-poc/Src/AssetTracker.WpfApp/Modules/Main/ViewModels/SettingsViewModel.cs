using AssetTracker.Application.Services;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.ViewModels;
using System.Collections.ObjectModel;
using System.Drawing;
using Xceed.Wpf.Toolkit;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ICacheManager _cacheManager;
        public SettingsViewModel(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;

            ConfigPath = _cacheManager.CacheFolderPath;
            SystemTags = new ObservableCollection<TagItem>(new List<TagItem>() {
                new TagItem { Name = "Game", Color = System.Windows.Media.Color.FromRgb(255,0,0) },
                new TagItem { Name = "Software", Color = System.Windows.Media.Color.FromRgb(0,0,255)}
            });

            OpenLocationCommand = new RelayCommand<string>(ExecuteOpenLocationCommand, CanExecuteOpenLocationCommand);
            PickSystemTagColorCommand = new RelayCommand<TagItem>(ExecutePickSystemTagColorCommand, CanPickSystemTagColorCommand);
        }

        string _configPath = string.Empty;
        public string ConfigPath
        {
            get => _configPath;
            set => SetProperty(ref _configPath, value);
        }

        ObservableCollection<TagItem> _systemTags = new ObservableCollection<TagItem>();
        public ObservableCollection<TagItem> SystemTags
        {
            get => _systemTags;
            set => SetProperty(ref _systemTags, value);
        }

        public IRelayCommand OpenLocationCommand { get; }
        public IRelayCommand PickSystemTagColorCommand { get; }


        private bool CanExecuteOpenLocationCommand(string path)
        {
            return WpfHelpers.CanOpenPath(path);
        }

        private void ExecuteOpenLocationCommand(string path)
        {
            WpfHelpers.OpenPath(path);
        }

        private bool CanPickSystemTagColorCommand(TagItem item) => true;

        private void ExecutePickSystemTagColorCommand(TagItem tag)
        {
            //if (tag == null) return;

            //var colorPicker = new ColorPicker();
            //colorPicker.Color = tag.Color;

            //var dialog = new ContentDialog
            //{
            //    Title = "Choose Tag Color",
            //    Content = colorPicker,
            //    PrimaryButtonText = "OK",
            //    SecondaryButtonText = "Cancel"
            //};

            //var result = await dialog.ShowAsync();
            //if (result == ContentDialogResult.Primary)
            //{
            //    tag.Color = colorPicker.Color;
            //    OnPropertyChanged(nameof(SystemTags));
            //}
        }
    }
}
