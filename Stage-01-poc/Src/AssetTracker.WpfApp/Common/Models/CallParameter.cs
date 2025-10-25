namespace AssetTracker.WpfApp.Common.Models
{
    /// <summary>
    /// Used for HttpClientAssetImporter params
    /// </summary>
    public class CallParameter : BindableBase
    {
        private string _key;
        public string Key
        {
            get=> _key; 
            set => SetProperty(ref _key, value);
        }

        string _label;
        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        string _value;
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}
