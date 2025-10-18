namespace AssetTracker.WpfApp.Common.Events
{    public class ChangeMainViewEvent
    {
        public string ServiceName { get; set; }
        public Type MainView { get; set; }
    }
}
