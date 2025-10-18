using AssetTracker.WpfApp.Common.Models.Enums;

namespace AssetTracker.WpfApp.Common.Models
{
    public class AssetItem : BindableBase
    {
        public AssetItem()
        {
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private AssetTypes _assetType;
        public AssetTypes AssetType
        {
            get => _assetType;
            set => SetProperty(ref _assetType, value);
        }
        private string[] _tags;
        public string[] Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }

    }
}
