using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Common.Views;
using System.Collections.ObjectModel;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IViewModel
    {
        public MainWindowViewModel()
        {
            _serviceViews = new ObservableCollection<IScraperServiceView>();
        }

        private ObservableCollection<IScraperServiceView> _serviceViews;
        public ObservableCollection<IScraperServiceView> ServiceViews
        {
            get => _serviceViews;
            private set { _serviceViews = value; OnPropertyChanged(nameof(ServiceViews)); }
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
            private set { _selectedServiceMainView = value; OnPropertyChanged(nameof(SelectedServiceMainView)); }
        }

        public void LoadScraperServices(IServiceProvider serviceProvider, IEnumerable<IScraperModule> modules)
        {
            foreach (var module in modules)
            {
                var view = module.GetView(serviceProvider);
                if (view != null)
                {
                    ServiceViews.Add(view);
                }
            }
        }
    }
}
