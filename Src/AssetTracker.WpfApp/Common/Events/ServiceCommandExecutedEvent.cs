namespace AssetTracker.WpfApp.Common.Events
{    public class ServiceCommandExecutedEvent
    {
        public string ServiceName { get; set; }
        public object CommandData { get; set; }
    }
}
