using AssetTracker.WpfApp.Common.Views;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    /// <summary>
    /// View
    /// </summary>
    public interface IScraperServiceMasterViewModel
    {
        IScraperServiceListItemView? ListItemView { get; }
        IScraperServiceMainView? DefaultMainView { get; }
    }
}
