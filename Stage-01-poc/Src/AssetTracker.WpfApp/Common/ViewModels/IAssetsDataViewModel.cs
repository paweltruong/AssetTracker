using AssetTracker.Core.Models;
using AssetTracker.WpfApp.Common.Commands;
using System.Collections.ObjectModel;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public interface IAssetsDataViewModel
    {
        ObservableCollection<OwnedAsset> OwnedAssets { get; set; }

        IRelayCommand ViewMarketplaceCommand { get; }
    }
}
