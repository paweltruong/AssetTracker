using AssetTracker.WpfApp.Common.Models.Enums;
using System.ComponentModel;

namespace AssetTracker.WpfApp.Common.Models
{
    public class ScraperServiceDataModel : INotifyPropertyChanged
    {
        private string _title;
        private string _description;
        private ScraperServiceStatus _status;
        private string _iconUrl;

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }

        public ScraperServiceStatus Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }
        public string IconUrl
        {
            get => _iconUrl;
            set { _iconUrl = value; OnPropertyChanged(nameof(IconUrl)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
