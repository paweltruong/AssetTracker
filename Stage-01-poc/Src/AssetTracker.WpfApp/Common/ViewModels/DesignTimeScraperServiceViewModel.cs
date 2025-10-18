using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class DesignTimeScraperServiceViewModel : ScraperServiceListItemViewModel<ScraperServiceDataModel>
    {
        public DesignTimeScraperServiceViewModel() : base(null)
        {
            // Set design-time data
            Model.Title = "Design Time Scraper Service";
            Model.Description = "This is a description for design time.";
            Model.Status =  ScraperServiceStatus.Running;
            Model.IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9d/Emblem-unreadable.svg/40px-Emblem-unreadable.svg.png";
            Model.ScrapeDataButtonText = "Scrape Data";
            Model.ViewDataButtonText = "View Data";
        }

        protected override void ExecuteConfigureServiceCommand(object parameter)
        {
        }
        protected override bool CanExecuteOpenFileCommand(object parameter)
        {
            return true;
        }
        protected override void ExecuteOpenFileCommand(object parameter)
        {
        }
        protected override bool CanExecuteSaveFileCommand(object parameter)
        {
            return true;
        }
        protected override void ExecuteSaveFileCommand(object parameter)
        {
        }

        protected override bool CanExecuteViewDataCommand(object parameter)
        {
            return true;
        }

        protected override void ExecuteViewDataCommand(object parameter)
        {
        }
    }
}
