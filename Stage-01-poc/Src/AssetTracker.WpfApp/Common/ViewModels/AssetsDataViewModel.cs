using AssetTracker.Core.Models;
using AssetTracker.Core.Services;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class AssetsDataViewModel : ViewModelBase, IAssetsDataViewModel, IDisposable
    {
        private readonly IPlugin _plugin;
        private readonly IAssetDatabase _assetDatabase;

        public string PluginKey => _plugin.PluginKey;
        public string PluginMarketplaceKey => _plugin.MarketplaceKey;


        public AssetsDataViewModel(IPlugin plugin, IAssetDatabase assetDatabase)
        {
            _plugin = plugin;
            _assetDatabase = assetDatabase;
            _assetDatabase.AssetDatabaseChanged += AssetDatabase_AssetDatabaseChanged;

            ViewMarketplaceCommand = new RelayCommand<string>(url => WpfHelpers.OpenUrl(url));

            _ = InitializeAsync();
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


        private async void AssetDatabase_AssetDatabaseChanged(object? sender, EventArgs e)
        {
            await UpdateAssetsAsync();
        }

        async Task UpdateAssetsAsync()
        {
            OwnedAssets = new ObservableCollection<OwnedAsset>(await _assetDatabase.GetAssetsForMarketplaceAsync(PluginMarketplaceKey));
        }

        public void Dispose()
        {
            _assetDatabase.AssetDatabaseChanged -= AssetDatabase_AssetDatabaseChanged;
        }

        private ICollectionView _sortedAssets;
        private string _sortColumn = "Name";
        private ListSortDirection _sortDirection = ListSortDirection.Ascending;

        private ObservableCollection<OwnedAsset> _ownedAssets = new ObservableCollection<OwnedAsset>();
        public ObservableCollection<OwnedAsset> OwnedAssets
        {
            get => _ownedAssets;
            set => SetProperty(ref _ownedAssets, value);
        }

        public IRelayCommand ViewMarketplaceCommand { get; }

    }
}
