using AssetTracker.Core.Models;
using AssetTracker.Core.Services;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class MyAssetsViewModel : ViewModelBase, IAssetsDataViewModel
    {
        private readonly IAssetDatabase _assetDatabase;
        public MyAssetsViewModel(IAssetDatabase assetDatabase)
        {
            _assetDatabase = assetDatabase;
            _assetDatabase.AssetDatabaseChanged += AssetDatabase_AssetDatabaseChanged;

            ViewMarketplaceCommand = new RelayCommand<string>(url => WpfHelpers.OpenUrl(url));

            _ = InitializeAsync();
        }

        private async void AssetDatabase_AssetDatabaseChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsDirty));
            await UpdateAssetsAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                await UpdateAssetsAsync();
            }
            catch (Exception ex)
            {
                // Handle initialization error appropriately
                MessageBox.Show($"Failed to initialize assets: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        async Task UpdateAssetsAsync()
        {
            OwnedAssets = new ObservableCollection<OwnedAsset>(await _assetDatabase.GetAllAssetsAsync());
        }

        private ObservableCollection<OwnedAsset> _ownedAssets = new ObservableCollection<OwnedAsset>();
        public ObservableCollection<OwnedAsset> OwnedAssets
        {
            get => _ownedAssets;
            set => SetProperty(ref _ownedAssets, value);
        }

        public bool IsDirty => _assetDatabase.IsDirty;

        public IRelayCommand ViewMarketplaceCommand { get; }
    }
}
