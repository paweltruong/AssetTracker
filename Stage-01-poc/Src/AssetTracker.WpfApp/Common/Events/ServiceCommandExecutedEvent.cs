namespace AssetTracker.WpfApp.Common.Events
{
    /// <summary>
    /// Used for tracking specific scraper service command executions.
    /// </summary>
    public class ServiceCommandExecutedEvent
    {
        public string ServiceName { get; set; }
        public object CommandData { get; set; }
        public DateTime? EventDate { get; set; }
    }
}
