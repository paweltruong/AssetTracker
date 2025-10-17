using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Common.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private IServiceProvider _serviceProvider;

        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe<ChangeMainViewEvent>(OnChangeMainViewEvent);
            _serviceViews = new ObservableCollection<IScraperServiceView>();
        }

        private ObservableCollection<IScraperServiceView> _serviceViews;
        public ObservableCollection<IScraperServiceView> ServiceViews
        {
            get => _serviceViews;
            private set { SetProperty(ref _serviceViews, value); }
        }

        IScraperServiceView? _selectedServiceView; public IScraperServiceView? SelectedServiceView
        {
            get => _selectedServiceView;
            set
            {
                _selectedServiceView = value;
                SelectedServiceMainView = value?.MainView;
                OnPropertyChanged(nameof(SelectedServiceView));
            }
        }

        IScraperServiceMainView? _selectedServiceMainView;
        public IScraperServiceMainView? SelectedServiceMainView
        {
            get => _selectedServiceMainView;
            private set { SetProperty(ref _selectedServiceMainView, value); }
        }

        public void LoadScraperServices(IServiceProvider serviceProvider, IEnumerable<IScraperModule> modules)
        {
            _serviceProvider = serviceProvider;

            foreach (var module in modules)
            {
                var view = module.GetView(serviceProvider);
                if (view != null)
                {
                    ServiceViews.Add(view);
                }
            }
        }

        private void OnChangeMainViewEvent(ChangeMainViewEvent eventData)
        {
            if(eventData != null 
                && eventData.MainView != null)
            {
                var newMainView = _serviceProvider.GetRequiredService(eventData.MainView) as IScraperServiceMainView;
                if(newMainView == null)
                {
                    throw new InvalidOperationException($"Could not resolve main view of type {eventData.MainView.FullName}");
                }
                SelectedServiceMainView = newMainView;
            }
        }
    }
}
