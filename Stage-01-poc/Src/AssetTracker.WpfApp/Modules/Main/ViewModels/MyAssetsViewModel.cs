using AssetTracker.Core.Models;
using AssetTracker.Core.Services;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Controls;
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
                OnPropertyChanged(nameof(SortSymbol));
                //OnPropertyChanged(nameof(SortIconAngle));
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
                OnPropertyChanged(nameof(SortSymbol));
                //OnPropertyChanged(nameof(SortIconAngle));
            }
        }

        // For character symbols (▲ ▼)
        public string SortSymbol => SortDirection == ListSortDirection.Ascending ? "▲" : "▼";

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
                string propertyName = columnHeader.Content.ToString();

                ListSortDirection direction = ListSortDirection.Ascending;

                // Your sorting logic here
                switch (propertyName)
                {
                    case "Name":
                        // Sort by Name property
                        if (_sortedColumn == "Name")
                        {
                            direction = _sortDirection == ListSortDirection.Descending ?
                                ListSortDirection.Ascending : ListSortDirection.Descending;
                        }
                        OwnedAssets = new ObservableCollection<OwnedAsset>(direction == ListSortDirection.Ascending ? OwnedAssets.OrderBy(a => a.Name) : OwnedAssets.OrderByDescending(a => a.Name));

                        SortDirection = direction;
                        SortedColumn = "Name";
                        break;
                    case "Asset":
                        // Sort by AssetType property
                        OwnedAssets = new ObservableCollection<OwnedAsset>(OwnedAssets.OrderBy(a => a.AssetType));
                        break;
                    case "Publishers":
                        // Sort by first publisher name or count
                        OwnedAssets = new ObservableCollection<OwnedAsset>(OwnedAssets.OrderBy(a => a.Publishers.FirstOrDefault()?.Name ?? ""));
                        break;
                        // Add other cases as needed
                }
            }
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
            var assetsFromDb = await _assetDatabase.GetAllAssetsAsync();
            OwnedAssets = new ObservableCollection<OwnedAsset>(assetsFromDb.OrderBy(x => x.Name).ToList());
        }

        private ObservableCollection<OwnedAsset> _ownedAssets = new ObservableCollection<OwnedAsset>();
        public ObservableCollection<OwnedAsset> OwnedAssets
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
