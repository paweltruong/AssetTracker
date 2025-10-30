using AssetTracker.Core.Models;
using AssetTracker.Core.Models.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AssetTracker.WpfApp.Common.Models
{
    public class AssetItem : OwnedAsset, INotifyPropertyChanged
    {
        private bool _selected;

        public AssetItem(
            string marketplaceUid,
            string name,
            AssetType assetType,
            HashSet<string> tags,
            string imageUrl,
            string assetUrl,
            IList<Publisher> publishers,
            IList<Developer> developers,
            string sourcePluginKey,
            string marketplaceKey,
            string marketplaceName,
            string marketplaceAccountId,
            string marketplaceUrl,
            HashSet<string> searchKeywords) 
            : base(
                  marketplaceUid,
                  name,
                  assetType,
                  tags,
                  imageUrl,
                  assetUrl,
                  publishers,
                  developers,
                  sourcePluginKey,
                  marketplaceKey,
                  marketplaceName,
                  marketplaceAccountId,
                  marketplaceUrl,
                  searchKeywords)
        {
        }

        public AssetItem(OwnedAsset asset)
        : base(
                  asset.MarketplaceUid,
                  asset.Name,
                  asset.AssetType,
                  asset.Tags,
                  asset.ImageUrl,
                  asset.AssetUrl,
                  asset.Publishers,
                  asset.Developers,
                  asset.SourcePluginKey,
                  asset.MarketplaceKey,
                  asset.MarketplaceName,
                  asset.MarketplaceAccountId,
                  asset.MarketplaceUrl,
                  asset.SearchKeywords)
        {            
        }

        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
