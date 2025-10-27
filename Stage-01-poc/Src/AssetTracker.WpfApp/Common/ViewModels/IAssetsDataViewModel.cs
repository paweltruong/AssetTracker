using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Models;
using System.Collections.ObjectModel;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public interface IAssetsDataViewModel
    {
        ObservableCollection<AssetItem> OwnedAssets { get; set; }

        IRelayCommand ViewMarketplaceCommand { get; }
    }
}
