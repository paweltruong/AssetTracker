using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using System.Windows.Input;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public abstract class ScraperServiceViewModel<TModel> : ViewModelBase, IScraperServiceViewModel where TModel : ScraperServiceDataModel, new()
    {
        protected readonly IEventAggregator _eventAggregator;
        protected TModel _model;
        public ScraperServiceViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _model = new TModel();

            // Initialize commands
            ConfigureCommand = new RelayCommand(ConfigureService);
            StopCommand = new RelayCommand(StopService, CanStopService);
            StartCommand = new RelayCommand(StartService, CanStartService);
        }

        // Expose model properties to View
        public virtual TModel Model 
        { 
            get => _model;
            protected set => SetProperty(ref _model, value);

        }

        // Commands
        public ICommand ConfigureCommand { get; }
        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        // Command methods
        protected abstract void ConfigureService(object parameter);
        protected abstract bool CanStopService(object parameter);
        protected abstract void StopService(object parameter);
        protected abstract bool CanStartService(object parameter);
        protected abstract void StartService(object parameter);

        protected virtual void OnServiceCommandExecuted(ServiceCommandExecutedEvent eventData)
        {
            // Handle the event at higher level
        }
    }
}

