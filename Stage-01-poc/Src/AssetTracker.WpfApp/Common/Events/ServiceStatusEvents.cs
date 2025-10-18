namespace AssetTracker.WpfApp.Common.Events
{
    /// <summary>
    /// Known events for service status updates.
    /// </summary>
    public enum ServiceStatusEvents
    {
        Start = 0,
        Success = 1,
        Failure = 2,
        Cancelled = 2,
    }
}
