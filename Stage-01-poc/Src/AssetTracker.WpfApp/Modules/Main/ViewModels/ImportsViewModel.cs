using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Common.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class ImportsViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IServiceProvider _serviceProvider;
        private readonly IViewModelFactory _viewModelFactory;

        public ImportsViewModel(IEventAggregator eventAggregator, IServiceProvider serviceProvider, IViewModelFactory viewModelFactory)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe<ChangeMainViewEvent>(OnChangeMainViewEvent);
            _serviceProvider = serviceProvider;
            _viewModelFactory = viewModelFactory;

            _scraperServices = new ObservableCollection<IScraperServiceMasterModel>();

            OpenLinkCommand = new RelayCommand<string>(url => WpfHelpers.OpenUrl(url));

            LoadAssetsImporterPlugins(App.Modules, App.Plugins);
        }

        #region Properties and Commands


        private ObservableCollection<IScraperServiceMasterModel> _scraperServices;
        public ObservableCollection<IScraperServiceMasterModel> ScraperServices
        {
            get => _scraperServices;
            private set { SetProperty(ref _scraperServices, value); }
        }

        IScraperServiceMainView? _selectedServiceMainView;
        public IScraperServiceMainView? SelectedServiceMainView
        {
            get => _selectedServiceMainView;
            private set { SetProperty(ref _selectedServiceMainView, value); }
        }


        IScraperServiceMasterModel? _selectedServiceView;
        public IScraperServiceMasterModel? SelectedServiceView
        {
            get => _selectedServiceView;
            set
            {
                _selectedServiceView = value;
                SelectedServiceMainView = value?.DefaultMainView;
                OnPropertyChanged(nameof(SelectedServiceView));
            }
        }

        public IRelayCommand OpenLinkCommand { get; }

        #endregion Properties and Commands

        public void LoadAssetsImporterPlugins(
            IEnumerable<IScraperModule> modules,
            List<Core.Services.Plugins.IPlugin> plugins)
        {
            foreach (var module in modules)
            {
                var view = module.GetMasterModel(_serviceProvider);
                if (view != null)
                {
                    ScraperServices.Add(view);
                }
            }
            foreach (var plugin in plugins)
            {
                if (plugin is IAssetsImporterPlugin assetsImporterPlugin)
                {
                    if (assetsImporterPlugin.UseDefaultBrowserLayout)
                    {
                        var model = CreateDefaultBrowserAssetsImporterMasterModel(assetsImporterPlugin);
                        if (model != null)
                        {
                            ScraperServices.Add(model);
                        }
                    }
                    else if (assetsImporterPlugin.UseDefaultHttpClientLayout)
                    {
                        var model = CreateDefaultHttpClientAssetsImporterMasterModel(assetsImporterPlugin);
                        if (model != null)
                        {
                            ScraperServices.Add(model);
                        }
                    }
                }
            }
        }


        private void OnChangeMainViewEvent(ChangeMainViewEvent eventData)
        {
            if (eventData != null
                && eventData.IsValid)
            {
                if (eventData.MainViewType != null)
                {
                    var newMainView = _serviceProvider.GetRequiredKeyedService(eventData.MainViewType, eventData.ServiceName) as IScraperServiceMainView;
                    if (newMainView == null)
                    {
                        throw new InvalidOperationException($"Could not resolve main view of type {eventData.MainViewType.FullName}");
                    }
                    if (newMainView is AssetsDataView assetsDataView)
                    {
                        if (assetsDataView.DataContext == null)
                        {
                            var assetsDataViewModel = _viewModelFactory.CreateAssetsDataViewModel(eventData.RelatedPlugin);
                            assetsDataView.DataContext = assetsDataViewModel;
                        }
                    }
                    SelectedServiceMainView = newMainView;
                }
                else if (eventData.MainView != null)
                {
                    SelectedServiceMainView = eventData.MainView;
                }
            }
        }


        private IScraperServiceMasterModel CreateDefaultBrowserAssetsImporterMasterModel(IAssetsImporterPlugin assetsImporterPlugin)
        {
            return new DefaultBrowserAssetsImporterMasterModel(_serviceProvider, assetsImporterPlugin);
        }
        private IScraperServiceMasterModel CreateDefaultHttpClientAssetsImporterMasterModel(IAssetsImporterPlugin assetsImporterPlugin)
        {
            return new DefaultHttpClientAssetsImporterMasterModel(_serviceProvider, assetsImporterPlugin);
        }

    }
}
