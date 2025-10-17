using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class DesignTimeScraperServiceViewModel : ScraperServiceViewModel<ScraperServiceDataModel>
    {
        public DesignTimeScraperServiceViewModel() : base(null)
        {
            // Set design-time data
            Model.Title = "Design Time Scraper Service";
            Model.Description = "This is a description for design time.";
            Model.Status =  ScraperServiceStatus.Running;
            Model.IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9d/Emblem-unreadable.svg/40px-Emblem-unreadable.svg.png";
        }

        protected override void ConfigureService(object parameter)
        {
        }
        protected override bool CanStopService(object parameter)
        {
            return true;
        }
        protected override void StopService(object parameter)
        {
        }
        protected override bool CanStartService(object parameter)
        {
            return true;
        }
        protected override void StartService(object parameter)
        {
        }

        protected override bool CanExecuteViewData(object parameter)
        {
            return true;
        }

        protected override void ExecuteViewData(object parameter)
        {
        }
    }
}
