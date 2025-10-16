using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;
using AssetTracker.WpfApp.Common.ViewModels;
using System.Windows;

namespace AssetTracker.WpfApp.Modules.SteamScraper.ViewModels
{
    public class SteamScraperServiceViewModel : ScraperServiceViewModel<ScraperServiceDataModel>
    {
        public SteamScraperServiceViewModel() : base()
        {
            Model.Title = "Steam";
            Model.Description = "Get owned game list";
            Model.IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/512px-Steam_icon_logo.svg.png";
            Model.Status = ScraperServiceStatus.NotLoaded;
        }


        // Command methods
        protected override void ConfigureService(object parameter)
        {
            // Open configuration dialog/window
            MessageBox.Show("Open configuration for scraper service");
        }
        protected override bool CanStopService(object parameter) => false;
        protected override void StopService(object parameter)
        {
            // Your business logic here

            // Update UI properties
            OnPropertyChanged(nameof(Model));

            // Refresh command states
            ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)StopCommand).RaiseCanExecuteChanged();

            MessageBox.Show("Scraper service stopped!");
        }
        protected override bool CanStartService(object parameter) => true;
        protected override void StartService(object parameter)
        {
            // Your business logic here
            _model.Status = ScraperServiceStatus.Running;

            // Update UI properties
            OnPropertyChanged(nameof(Model));

            // Refresh command states
            ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)StopCommand).RaiseCanExecuteChanged();

            MessageBox.Show("Scraper service started!");
        }
    }
}
