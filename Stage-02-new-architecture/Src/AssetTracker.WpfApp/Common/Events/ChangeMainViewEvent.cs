using AssetTracker.Core.Services.Plugins;
using AssetTracker.WpfApp.Common.Views;

namespace AssetTracker.WpfApp.Common.Events
{
    public class ChangeMainViewEvent
    {
        public ChangeMainViewEvent(IPlugin plugin, Type? viewType)
        {
            RelatedPlugin = plugin;
            ServiceName = plugin.PluginKey;
            MainViewType = viewType;
        }
        public ChangeMainViewEvent(IPlugin plugin, IScraperServiceMainView? view)
        {
            RelatedPlugin = plugin;
            ServiceName = plugin.PluginKey;
            MainView = view;
        }

        [Obsolete("Modules should be refactored into plugins")]
        public ChangeMainViewEvent(string moduleName, Type? viewType)
        {
            ServiceName = moduleName;
            MainViewType = viewType;
        }


        public string ServiceName { get; set; }
        public IPlugin RelatedPlugin { get; set; }
        public Type? MainViewType { get; set; }
        public IScraperServiceMainView? MainView { get; set; }

        public bool IsValid => !string.IsNullOrWhiteSpace(ServiceName) && (MainViewType != null || MainView != null);
    }
}
