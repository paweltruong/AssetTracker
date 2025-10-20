using AssetTracker.Core.Models.Enums;
using AssetTracker.Core.Services.AssetsComparer;
using AssetTracker.Core.Services.AssetsResolver;
using AssetTracker.Core.Services.AssetsResolver.Definitions;
using AssetTracker.WpfApp.Common;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Common.Views;
using AssetTracker.WpfApp.Modules.SteamScraper;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IAssetsComparer _assetsComparer;

        private CancellationTokenSource _lastCheckLinkCancellationTokenSource;
        private IServiceProvider _serviceProvider;

        public MainWindowViewModel(IEventAggregator eventAggregator, IAssetsComparer assetsComparer)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe<ChangeMainViewEvent>(OnChangeMainViewEvent);
            _eventAggregator.Subscribe<CheckLinkStatusChangedEvent>(OnCheckLinkStatusChangedEvent);
            _assetsComparer = assetsComparer;

            _scraperServices = new ObservableCollection<IScraperServiceMasterModel>();

            CheckLinkCommand = new AsyncRelayCommand(ExecuteCheckLinkCommand, CanExecuteCheckLinkCommand);
            CancelCheckLinkCommand = new AsyncRelayCommand(ExecuteCancelCheckLinkCommand, CanExecuteCancelCheckLinkCommand);
            OpenLinkCommand = new RelayCommand<string>(url =>
            {
                if (!string.IsNullOrEmpty(url))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
            });
        }

        private async void OnCheckLinkStatusChangedEvent(CheckLinkStatusChangedEvent @event)
        {
            if (@event.CheckLinkProcessStatus == GenericProcessStatus.InProgress)
            {
                CheckLinkStatusMessage = "Checking link...";
            }
            else if (@event.CheckLinkProcessStatus == GenericProcessStatus.Completed)
            {
                //await Task.Run(() =>
                //{

                //    CheckLinkStatusMessage = "Link check completed.";
                //    var newResults = new ObservableCollection<AssetComparisonResult>(ComparisonResults);

                //    Parallel.ForEach(newResults, result =>
                //    {
                //        var compareResult = _assetsComparer.CompareAssetAsync(result.InnerItem);
                //        if (compareResult.WasSuccessful)
                //        {
                //            result.Status = compareResult.MatchingOwnedAssets.Any() ?
                //                AssetComparisonStatus.Exists : AssetComparisonStatus.NotExists;
                //        }
                //        else
                //        {
                //            result.Status = AssetComparisonStatus.Error;
                //        }
                //    });


                //    //using var cts = new CancellationTokenSource();
                //    //var cancellationToken = cts.Token;
                //    //await Parallel.ForEachAsync(newResults, cts.Token, async (result, cancellationToken) =>
                //    //{
                //    //    var compareResult = await _assetsComparer.CompareAssetAsync(result.InnerItem, cancellationToken);
                //    //    if (compareResult.WasSuccessful)
                //    //    {
                //    //        result.Status = compareResult.MatchingOwnedAssets.Any() ?
                //    //            AssetComparisonStatus.Exists : AssetComparisonStatus.NotExists;
                //    //    }
                //    //    else
                //    //    {
                //    //        result.Status = AssetComparisonStatus.Error;
                //    //    }
                //    //});

                //    //foreach (var item in newResults)
                //    //{
                //    //    var compareResult = await _assetsComparer.CompareAssetAsync(item.InnerItem, cancellationToken);
                //    //    if (compareResult.WasSuccessful)
                //    //    {
                //    //        item.Status = compareResult.MatchingOwnedAssets.Any() ?
                //    //            AssetComparisonStatus.Exists : AssetComparisonStatus.NotExists;
                //    //    }
                //    //    else
                //    //    {
                //    //        item.Status = AssetComparisonStatus.Error;
                //    //    }
                //    //}

                //    // Execute on UI thread if needed
                //    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                //    {
                //        ComparisonResults = newResults;
                //    });
                //});

            }
            else if (@event.CheckLinkProcessStatus == GenericProcessStatus.Failed)
            {
                CheckLinkStatusMessage = "Link check failed.";
            }
            else if (@event.CheckLinkProcessStatus == GenericProcessStatus.Canceled)
            {
                CheckLinkStatusMessage = "Link check canceled.";
            }
        }

        private bool CanExecuteCancelCheckLinkCommand()
        {
            return IsCheckLinkInProgress;
        }

        private async Task ExecuteCancelCheckLinkCommand()
        {
            if (!IsCheckLinkInProgress
               || _lastCheckLinkCancellationTokenSource == null)
            {
                System.Diagnostics.Debug.WriteLine($"Error cannot cancel CheckLink");
                return;
            }

            try
            {
                await _lastCheckLinkCancellationTokenSource.CancelAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error while cancelling scraping {SteamScraperModule.ModuleName}: {ex.Message}");
            }
            CheckLinkStatusMessage = "Scraping cancelled by user.";
            _eventAggregator.Publish(new CheckLinkStatusChangedEvent
            {
                CheckLinkProcessStatus = GenericProcessStatus.Canceled,
            });
        }

        private bool CanExecuteCheckLinkCommand()
        {
            return !IsCheckLinkInProgress && HttpHelpers.IsValidHttpUrl(CheckLinkUrl);
        }

        List<Task<AssetsResolverResult>> _ongoingTasks = new List<Task<AssetsResolverResult>>();
        bool _isCheckLinkInProgress;
        public bool IsCheckLinkInProgress
        {
            get => _isCheckLinkInProgress;
            private set
            {
                SetProperty(ref _isCheckLinkInProgress, value);
                CheckLinkCommand.RaiseCanExecuteChanged();
                CancelCheckLinkCommand.RaiseCanExecuteChanged();
            }
        }

        private async Task ExecuteCheckLinkCommand()
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("Service provider is not set. Cannot execute CheckLinkCommand.");
            }

            string url = CheckLinkUrl;
            _ongoingTasks.Clear();
            _lastCheckLinkCancellationTokenSource = new CancellationTokenSource();
            IsCheckLinkInProgress = true;
            _eventAggregator.Publish(new CheckLinkStatusChangedEvent
            {
                CheckLinkProcessStatus = GenericProcessStatus.InProgress,
            });

            try
            {
                foreach (var service in ScraperServices)
                {
                    var assetUrlResolve = _serviceProvider.GetRequiredKeyedService<IAssetsResolver>(service.ModuleName);
                    if (assetUrlResolve != null)
                    {
                        _ongoingTasks.Add(assetUrlResolve.ResolveAssetFromUrlAsync(url, _lastCheckLinkCancellationTokenSource.Token));
                    }
                }

                await Task.WhenAll(_ongoingTasks.ToArray());

                var successfullResolvers = _ongoingTasks.Where(t => t.Result.WasSuccessful);

                if (!successfullResolvers.Any())
                {
                    CheckLinkStatusMessage = "No relevant service found for the provided URL.";
                    return;
                }

                ComparisonResults = new ObservableCollection<AssetComparisonResult>(
                    successfullResolvers.SelectMany(t => t.Result.Items.Select(a => new AssetComparisonResult(a)))
                );
                //await _assetsComparer.CompareAsync(_lastCheckLinkCancellationTokenSource.Token);

                _eventAggregator.Publish(new CheckLinkStatusChangedEvent
                {
                    CheckLinkProcessStatus = GenericProcessStatus.Completed,
                });
            }
            catch (OperationCanceledException)
            {
                CheckLinkStatusMessage = "Check link operation was cancelled.";
                _eventAggregator.Publish(new CheckLinkStatusChangedEvent
                {
                    CheckLinkProcessStatus = GenericProcessStatus.Canceled
                });
            }
            catch (Exception ex)
            {
                CheckLinkStatusMessage = $"An error occurred during link checking: {ex.Message}";
                _eventAggregator.Publish(new CheckLinkStatusChangedEvent
                {
                    CheckLinkProcessStatus = GenericProcessStatus.Failed
                });
            }
            finally
            {
                _lastCheckLinkCancellationTokenSource.Dispose();
                IsCheckLinkInProgress = false;
            }
        }

        string _checkLinkUrl;
        public string CheckLinkUrl
        {
            get => _checkLinkUrl;
            set
            {
                SetProperty(ref _checkLinkUrl, value);
                CheckLinkCommand.RaiseCanExecuteChanged();
            }
        }

        string _checkLinkStatusMessage;
        public string CheckLinkStatusMessage
        {
            get => _checkLinkStatusMessage;
            protected set => SetProperty(ref _checkLinkStatusMessage, value);
        }

        private ObservableCollection<AssetComparisonResult> _comparisonResults = new ObservableCollection<AssetComparisonResult>();
        public ObservableCollection<AssetComparisonResult> ComparisonResults
        {
            get => _comparisonResults;
            private set { SetProperty(ref _comparisonResults, value); }
        }

        private ObservableCollection<IScraperServiceMasterModel> _scraperServices;
        public ObservableCollection<IScraperServiceMasterModel> ScraperServices
        {
            get => _scraperServices;
            private set { SetProperty(ref _scraperServices, value); }
        }

        IScraperServiceMasterModel? _selectedServiceView; public IScraperServiceMasterModel? SelectedServiceView
        {
            get => _selectedServiceView;
            set
            {
                _selectedServiceView = value;
                SelectedServiceMainView = value?.DefaultMainView;
                OnPropertyChanged(nameof(SelectedServiceView));
            }
        }

        IScraperServiceMainView? _selectedServiceMainView;
        public IScraperServiceMainView? SelectedServiceMainView
        {
            get => _selectedServiceMainView;
            private set { SetProperty(ref _selectedServiceMainView, value); }
        }

        public IRelayCommand OpenLinkCommand { get; }
        public IRelayCommand CheckLinkCommand { get; }
        public IRelayCommand CancelCheckLinkCommand { get; }

        public void LoadScraperServices(IServiceProvider serviceProvider, IEnumerable<IScraperModule> modules)
        {
            _serviceProvider = serviceProvider;
            foreach (var module in modules)
            {
                var view = module.GetMasterModel(serviceProvider);
                if (view != null)
                {
                    ScraperServices.Add(view);
                }
            }
        }

        private void OnChangeMainViewEvent(ChangeMainViewEvent eventData)
        {
            if (eventData != null
                && eventData.MainView != null)
            {
                var newMainView = _serviceProvider.GetRequiredService(eventData.MainView) as IScraperServiceMainView;
                if (newMainView == null)
                {
                    throw new InvalidOperationException($"Could not resolve main view of type {eventData.MainView.FullName}");
                }
                SelectedServiceMainView = newMainView;
            }
        }
    }
}
