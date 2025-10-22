using AssetTracker.Core.Models.Enums;
using AssetTracker.Core.Services.AssetsComparer;
using AssetTracker.Core.Services.AssetsResolver;
using AssetTracker.Core.Services.AssetsResolver.Definitions;
using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Commands;
using AssetTracker.WpfApp.Common.Events;
using AssetTracker.WpfApp.Common.Models;
using AssetTracker.WpfApp.Common.Services;
using AssetTracker.WpfApp.Common.Utils;
using AssetTracker.WpfApp.Common.ViewModels;
using AssetTracker.WpfApp.Modules.SteamScraper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracker.WpfApp.Modules.Main.ViewModels
{
    public class CheckAssetsViewModel : ViewModelBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly IAssetsComparer _assetsComparer;

        private CancellationTokenSource _lastCheckLinkCancellationTokenSource;

        public CheckAssetsViewModel(IServiceProvider serviceProvider, IEventAggregator eventAggregator, IAssetsComparer assetsComparer)
        {
            _serviceProvider = serviceProvider;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe<CheckLinkStatusChangedEvent>(OnCheckLinkStatusChangedEvent);
            _assetsComparer = assetsComparer;

            CheckLinkCommand = new AsyncRelayCommand(ExecuteCheckLinkCommand, CanExecuteCheckLinkCommand);
            CancelCheckLinkCommand = new AsyncRelayCommand(ExecuteCancelCheckLinkCommand, CanExecuteCancelCheckLinkCommand);
            _eventAggregator = eventAggregator;
            _assetsComparer = assetsComparer;
        }

        #region Propeties and Commands

        public IRelayCommand CheckLinkCommand { get; }
        public IRelayCommand CancelCheckLinkCommand { get; }

        #endregion Properties and Commands

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
                foreach (var plugin in App.Plugins)
                {
                    if (plugin is IAssetsResolverPlugin)
                    {
                        var assetUrlResolve = _serviceProvider.GetRequiredKeyedService<IAssetsResolver>(plugin.PluginKey);
                        if (assetUrlResolve != null)
                        {
                            _ongoingTasks.Add(assetUrlResolve.ResolveAssetFromUrlAsync(url, _lastCheckLinkCancellationTokenSource.Token));
                        }
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


        private async void OnCheckLinkStatusChangedEvent(CheckLinkStatusChangedEvent @event)
        {
            if (@event.CheckLinkProcessStatus == GenericProcessStatus.InProgress)
            {
                CheckLinkStatusMessage = "Checking link...";
            }
            else if (@event.CheckLinkProcessStatus == GenericProcessStatus.Completed)
            {
                foreach (var result in ComparisonResults)
                {
                    result.Status = AssetComparisonStatus.Processing;
                }

                // Process each item in parallel and update status independently
                var tasks = ComparisonResults.Select(async result =>
                {
                    try
                    {
                        var compareResult = await _assetsComparer.CompareAssetAsync(result.InnerItem, CancellationToken.None);

                        // Use Dispatcher to update UI properties on the UI thread
                        await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            if (compareResult.WasSuccessful)
                            {
                                result.Status = compareResult.MatchingOwnedAssets.Any() ?
                                    AssetComparisonStatus.Exists : AssetComparisonStatus.NotExists;
                            }
                            else
                            {
                                result.Status = AssetComparisonStatus.Error;
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        await System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            result.Status = AssetComparisonStatus.Error;
                        });
                        Debug.WriteLine($"Error comparing asset: {ex.Message}");
                    }
                });

                await Task.WhenAll(tasks);
                CheckLinkStatusMessage = "All comparisons completed.";

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


    }
}
