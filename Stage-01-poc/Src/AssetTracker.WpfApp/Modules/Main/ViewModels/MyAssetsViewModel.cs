using AssetTracker.Core.Models;
using AssetTracker.Core.Services;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.ViewModels;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class MyAssetsViewModel : ViewModelBase, IAssetsDataViewModel
    {
        private readonly IAssetDatabase _assetDatabase;

        ListSortDirection _sortDirection = ListSortDirection.Ascending;
        public ListSortDirection SortDirection
        {
            get => _sortDirection;
            set
            {
                _sortDirection = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SortIconAngle));
            }
        }
        string _sortedColumn = "Name";
        public string SortedColumn
        {
            get => _sortedColumn;
            set
            {
                _sortedColumn = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SortIconAngle));
            }
        }

        string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                SetProperty(ref _filterText, value);
                UpdateAssetsAsync();
            }
        }

        // For icon rotation (0° for ascending, 180° for descending)
        public double SortIconAngle => SortDirection == ListSortDirection.Ascending ? 0 : 180;

        public MyAssetsViewModel(IAssetDatabase assetDatabase)
        {
            _assetDatabase = assetDatabase;
            _assetDatabase.AssetDatabaseChanged += AssetDatabase_AssetDatabaseChanged;

            ViewMarketplaceCommand = new RelayCommand<string>(url => WpfHelpers.OpenUrl(url));
            SaveCommand = new AsyncRelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
            SortCommand = new RelayCommand<object>(ExecuteSortCommand);


            _ = InitializeAsync();
        }

        private void ExecuteSortCommand(object parameter)
        {
            if (parameter is GridViewColumnHeader columnHeader)
            {
                string columnName = columnHeader.Column?.Header?.ToString();

                if (columnName == SortedColumn)
                {
                    // Toggle direction if same column
                    SortDirection = SortDirection == ListSortDirection.Ascending
                        ? ListSortDirection.Descending
                        : ListSortDirection.Ascending;
                }
                else
                {
                    SortedColumn = columnName;
                    SortDirection = ListSortDirection.Ascending;
                }

                // Apply sorting based on current SortedColumn and SortDirection
                ApplySorting();
            }
        }
        private void ApplySorting()
        {
            if (OwnedAssets == null || !OwnedAssets.Any()) return;

            var sortedAssets = SortDirection == ListSortDirection.Ascending
                ? OwnedAssets.OrderBy(GetSortProperty).ToList()
                : OwnedAssets.OrderByDescending(GetSortProperty).ToList();

            OwnedAssets = new ObservableCollection<AssetItem>(sortedAssets);
        }

        private object GetSortProperty(OwnedAsset asset)
        {
            return SortedColumn switch
            {
                "Name" => asset.Name,
                "Asset" => asset.AssetType,
                "Publishers" => asset.Publishers?.FirstOrDefault()?.Name ?? "",
                "Developers" => asset.Developers?.FirstOrDefault()?.Name ?? "",
                "Tags" => asset.Tags?.FirstOrDefault() ?? "",
                "Marketplace" => asset.MarketplaceName,
                _ => asset.Name
            };
        }

        private bool CanExecuteSaveCommand() => IsDirty;

        private async Task ExecuteSaveCommand()
        {
            await _assetDatabase.SaveAsync();
        }

        private async void AssetDatabase_AssetDatabaseChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsDirty));
            SaveCommand.RaiseCanExecuteChanged();

            await UpdateAssetsAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                await _assetDatabase.LoadAllAssetsAsync();
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
            //TODO:opmtimization
            var assetsFromDb = await _assetDatabase.GetAllAssetsAsync();

            //Store selected items before refresh
            HashSet<int> selectedAssets = new HashSet<int>();
            foreach (var asset in OwnedAssets.Where(a => a.Selected))
            {
                selectedAssets.Add(asset.GetHashCode());
            }

            if (string.IsNullOrEmpty(FilterText))
            {
                OwnedAssets = new ObservableCollection<AssetItem>(assetsFromDb.OrderBy(x => x.Name).Select(asset => new AssetItem(asset)));
            }
            else
            {
                OwnedAssets = new ObservableCollection<AssetItem>(
                    assetsFromDb.Where(x => x.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                    || x.MarketplaceName.Contains(FilterText, StringComparison.OrdinalIgnoreCase)
                    || x.Developers.Any(d => d.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                    || x.Publishers.Any(p => p.Name.Contains(FilterText, StringComparison.OrdinalIgnoreCase))
                    )
                    .OrderBy(x => x.Name).Select(asset => new AssetItem(asset)));
            }

            //Restore selections
            foreach (var item in OwnedAssets)
            {
                if (selectedAssets.Contains(item.GetHashCode())) item.Selected = true;
            }
        }

        private ObservableCollection<AssetItem> _ownedAssets = new ObservableCollection<AssetItem>();
        public ObservableCollection<AssetItem> OwnedAssets
        {
            get => _ownedAssets;
            set => SetProperty(ref _ownedAssets, value);
        }

        public bool IsDirty => _assetDatabase.IsDirty;

        public IRelayCommand ViewMarketplaceCommand { get; }
        public IRelayCommand SaveCommand { get; }
        public IRelayCommand SortCommand { get; }


    }
}
