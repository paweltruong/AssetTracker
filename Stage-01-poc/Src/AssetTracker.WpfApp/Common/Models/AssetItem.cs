using AssetTracker.Core.Models.Enums;
using AssetTracker.WpfApp.Common.Models.Enums;
using System.Collections.Generic;
using System.IO.Pipes;

namespace AssetTracker.WpfApp.Common.Models
{
    //TODO:split plain domain and WPF models
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
        private AssetType _assetType;
        public AssetType AssetType
        {
            get => _assetType;
            set => SetProperty(ref _assetType, value);
        }
        private HashSet<string> _tags;
        public HashSet<string> Tags
        {
            get => _tags;
            set => SetProperty(ref _tags, value);
        }

        private string _imageUrl;
        public string ImageUrl
        {
            get => _imageUrl;
            set => SetProperty(ref _imageUrl, value);
        }

        private PublisherItem[] _publishers;
        public PublisherItem[] Publishers
        {
            get => _publishers;
            set => SetProperty(ref _publishers, value);
        }
        private DeveloperItem[] _developers;
        public DeveloperItem[] Developers
        {
            get => _developers;
            set => SetProperty(ref _developers, value);
        }
        private string _assetUrl;
        public string AssetUrl
        {
            get => _assetUrl;
            set => SetProperty(ref _assetUrl, value);
        }

    }

    public class PublisherItem
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
    }

    public class DeveloperItem
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
    }
}
