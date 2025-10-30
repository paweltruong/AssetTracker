using AssetTracker.Core.Services;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    public abstract class ScraperServiceListItemViewModel<TModel> : ViewModelBase, IScraperServiceViewModel where TModel : ScraperServiceDataModel, new()
    {
        protected readonly IEventAggregator _eventAggregator;
        protected readonly IAssetDatabase _assetDatabase;
        protected TModel _model;
        public ScraperServiceListItemViewModel(IEventAggregator eventAggregator, IAssetDatabase assetDatabase)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe<ServiceCommandExecutedEvent>(OnServiceCommandExecuted);
            _eventAggregator.Subscribe<ServiceDataChangedEvent>(OnServiceDataChanged);
            _assetDatabase = assetDatabase;
            _assetDatabase.AssetDatabaseChanged += AssetDatabase_AssetDatabaseChanged;
            _assetDatabase.AssetDatabaseLoaded += AssetDatabase_AssetDatabaseLoaded;
            _model = new TModel();

            // Initialize commands
            ConfigureCommand = new RelayCommand(ExecuteConfigureServiceCommand);
            OpenFileCommand = new RelayCommand(ExecuteOpenFileCommand, CanExecuteOpenFileCommand);
            SaveFileCommand = new RelayCommand(ExecuteSaveFileCommand, CanExecuteSaveFileCommand);
            ViewDataCommand = new RelayCommand(ExecuteViewDataCommand, CanExecuteViewDataCommand);
        }

        protected virtual void AssetDatabase_AssetDatabaseLoaded(object? sender, EventArgs e)
        {
        }

        protected virtual void AssetDatabase_AssetDatabaseChanged(object? sender, EventArgs e)
        {
        }

        // Expose model properties to View
        public virtual TModel Model 
        { 
            get => _model;
            protected set => SetProperty(ref _model, value);

        }

        // Commands
        public IRelayCommand ConfigureCommand { get; }
        public IRelayCommand SaveFileCommand { get; }
        public IRelayCommand OpenFileCommand { get; }
        public IRelayCommand ViewDataCommand { get; }

        // Command methods
        protected abstract void ExecuteConfigureServiceCommand(object parameter);
        protected abstract bool CanExecuteOpenFileCommand(object parameter);
        protected abstract void ExecuteOpenFileCommand(object parameter);
        protected abstract bool CanExecuteSaveFileCommand(object parameter);
        protected abstract void ExecuteSaveFileCommand(object parameter);


        protected abstract bool CanExecuteViewDataCommand(object parameter);
        protected abstract void ExecuteViewDataCommand(object parameter);

        protected virtual void OnServiceCommandExecuted(ServiceCommandExecutedEvent eventData)
        {
            // Handle the event at higher level
        }

        protected virtual void OnServiceDataChanged(ServiceDataChangedEvent eventData)
        {
            // Handle the event at higher level
        }
    }
}

