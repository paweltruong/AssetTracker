using AssetTracker.WpfApp.Common.Models.Enums;

namespace AssetTracker.WpfApp.Common.Models
{
    public class ScraperServiceDataModel : BindableBase
    {
        public const string ViewDataButtonTextDefault = "View Data";
        public const string ViewDataButtonTextFormatted = "View Data ({0})";

        private string _title;
        private string _description;
        private ScraperServiceStatus _status = ScraperServiceStatus.Unknown;
        private string _iconUrl;
        private string _scrapeDataButtonText = "Scrape Data";
        private string _viewDataButtonText = ViewDataButtonTextDefault;
        private int _dataCount = 0;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ScraperServiceStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        public string IconUrl
        {
            get => _iconUrl;
            set => SetProperty(ref _iconUrl, value);
        }
        public string ScrapeDataButtonText
        {

            get => _scrapeDataButtonText;
            set => SetProperty(ref _scrapeDataButtonText, value);
        }
        public string ViewDataButtonText
        {

            get => _viewDataButtonText;
            set => SetProperty(ref _viewDataButtonText, value);
        }

        public int DataCount
        {
            get => _dataCount;
            set => SetProperty(ref _dataCount, value);
        }
    }
}
