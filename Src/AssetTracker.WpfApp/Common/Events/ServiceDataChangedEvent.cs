namespace AssetTracker.WpfApp.Common.Events
{
    /// <summary>
    /// Used for tracking specific scraper service command executions.
    /// </summary>
    public class ServiceDataChangedEvent
    {
        public string ServiceName { get; set; }
        public int DataCount { get; set; }
    }
}
