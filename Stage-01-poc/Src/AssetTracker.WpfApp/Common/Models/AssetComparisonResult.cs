using AssetTracker.Core.Models;
using AssetTracker.Core.Models.Enums;

namespace AssetTracker.WpfApp.Common.Models
{
    public class AssetComparisonResult : BindableBase
    {

        public AssetComparisonResult(Asset innerItem)
        {
            _innerItem = innerItem;
        }

        Asset _innerItem;
        public Asset InnerItem => _innerItem;
        public string? ImageUrl => _innerItem.ImageUrl;
        public string Name => _innerItem.Name;
        public AssetType AssetType => _innerItem.AssetType;
        public string? AssetUrl => _innerItem.AssetUrl;


        private AssetComparisonStatus _status;
        public AssetComparisonStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        OwnedAssetMatch? _bestMatch;
        public OwnedAssetMatch? BestMatch
        {
            get => _bestMatch;
            set => SetProperty(ref _bestMatch, value);
        }
    }
}
