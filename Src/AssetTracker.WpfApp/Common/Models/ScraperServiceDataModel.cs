using AssetTracker.WpfApp.Common.Models.Enums;
using System.ComponentModel;

namespace AssetTracker.WpfApp.Common.Models
{
    public class ScraperServiceDataModel : BindableBase
    {
        private string _title;
        private string _description;
        private ScraperServiceStatus _status = ScraperServiceStatus.Unknown;
        private string _iconUrl;
        private string _scrapeDataButtonText = "Scrape Data";

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
    }
}
