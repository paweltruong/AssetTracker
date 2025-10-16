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
        public DesignTimeScraperServiceViewModel()
        {
            // Set design-time data
            // Assuming the base class has properties like Title, Description, Status, etc.
            // You can set them here for design-time visualization
            // Example:
            this.Title = "Design Time Scraper Service";
            this.Description = "This is a description for design time.";
            this.Status =  ScraperServiceStatus.Running;
            this.IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/9/9d/Emblem-unreadable.svg/40px-Emblem-unreadable.svg.png";
        }

        public override string Title { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
        public override string Description { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
        public override ScraperServiceStatus Status { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
        public override string IconUrl { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }

        protected override void ConfigureService(object parameter)
        {
            throw new NotImplementedException();
        }
        protected override bool CanStartService(object parameter)
        {
            throw new NotImplementedException();
        }
        protected override void StartService(object parameter)
        {
            throw new NotImplementedException();
        }
        protected override bool CanStopService(object parameter)
        {
            throw new NotImplementedException();
        }
        protected override void StopService(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
