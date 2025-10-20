using AssetTracker.WpfApp.Common.Models.Enums;

namespace AssetTracker.WpfApp.Common.Models
{
    public class AssetComparisonResult : BindableBase
    {
        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private AssetType _assetType;
        public AssetType AssetType
        {
            get => _assetType;
            set => SetProperty(ref _assetType, value);
        }

        private AssetComparisonStatus _status;
        public AssetComparisonStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        private string _assetUrl;
        public string AssetUrl
        {
            get => _assetUrl;
            set => SetProperty(ref _assetUrl, value);
        }
    }
}
