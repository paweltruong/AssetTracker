using AssetTracker.WpfApp.Common.Views;

namespace AssetTracker.WpfApp.Common.Events
{    public class ChangeMainViewEvent
    {
        public string ServiceName { get; set; }
        public Type? MainViewType { get; set; }
        public IScraperServiceMainView? MainView { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(ServiceName) && (MainViewType != null || MainView != null);
    }
}
