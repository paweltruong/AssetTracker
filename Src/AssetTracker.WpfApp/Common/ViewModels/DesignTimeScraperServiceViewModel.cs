using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public class DesignTimeScraperServiceViewModel : ScraperServiceViewModel<ScraperServiceDataModel>
    {
        public DesignTimeScraperServiceViewModel() : base(null)
        {
            // Set design-time data
            // Assuming the base class has properties like Title, Description, Status, etc.
            // You can set them here for design-time visualization
            // Example:
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
    }
}
