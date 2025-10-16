using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Models.Enums;
using AssetTracker.WpfApp.Common.ViewModels;
using System.Windows;

namespace AssetTracker.WpfApp.Modules.SteamScraper.ViewModels
{
    public class SteamScraperServiceViewModel : ScraperServiceViewModel<ScraperServiceDataModel>
    {
        //https://pl.m.wikipedia.org/wiki/Plik:Steam_icon_logo.svg

        public SteamScraperServiceViewModel() : base()
        {
            Title = "Steam Scraper";
            Description = "Scrapes game data from Steam.";
            IconUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/512px-Steam_icon_logo.svg.png";
            Status = ScraperServiceStatus.Unknown;
        }

        // Expose model properties to View
        public override string Title
        {
            get { return _model.Title; }
            protected set
            {
                _model.Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public override string Description { get { return _model.Description; } protected set { _model.Description = value; } }
        public override ScraperServiceStatus Status
        {
            get
            {
                return _model.Status;
            }
            protected set
            {
                _model.Status = value;
                //                OnPropertyChanged(nameof(Status));
            }
        }
        public override string IconUrl
        {
            get { return _model.IconUrl; }
            protected set
            {
                _model.IconUrl = value;
                OnPropertyChanged(nameof(IconUrl));
            }
        }


        // Command methods
        protected override void ConfigureService(object parameter)
        {
            // Open configuration dialog/window
            MessageBox.Show("Open configuration for scraper service");
        }
        protected override bool CanStartService(object parameter) => true;
        protected override void StartService(object parameter)
        {
            // Your business logic here
            _model.Status = ScraperServiceStatus.Running;

            // Update UI properties
            OnPropertyChanged(nameof(Status));

            // Refresh command states
            ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)StopCommand).RaiseCanExecuteChanged();

            MessageBox.Show("Scraper service started!");
        }
        protected override bool CanStopService(object parameter) => false;
        protected override void StopService(object parameter)
        {
            // Your business logic here

            // Update UI properties
            OnPropertyChanged(nameof(Status));

            // Refresh command states
            ((RelayCommand)StartCommand).RaiseCanExecuteChanged();
            ((RelayCommand)StopCommand).RaiseCanExecuteChanged();

            MessageBox.Show("Scraper service stopped!");
        }
    }
}
