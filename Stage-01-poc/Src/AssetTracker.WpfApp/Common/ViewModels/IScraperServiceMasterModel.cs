using AssetTracker.WpfApp.Common.Views;

namespace AssetTracker.WpfApp.Common.ViewModels
{
    /// <summary>
    /// View
    /// </summary>
    public interface IScraperServiceMasterModel
    {
        string ModuleName { get; }
        IScraperServiceListItemView? ListItemView { get; }
        IScraperServiceMainView? DefaultMainView { get; }
        IScraperServiceMainView? ImportAssetsView { get; }
    }
}
