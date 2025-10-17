using AssetTracker.WpfApp.Common.ViewModels;

namespace AssetTracker.WpfApp.Common.Views
{
    public interface IScraperServiceView
    {
        IScraperServiceListItemView? ListItemView { get; }
        IScraperServiceMainView? MainView { get; }
    }

    public interface IScraperServiceView<TViewModel> : IScraperServiceView, IView<TViewModel> where TViewModel : IScraperServiceViewModel
    {
    }
}
