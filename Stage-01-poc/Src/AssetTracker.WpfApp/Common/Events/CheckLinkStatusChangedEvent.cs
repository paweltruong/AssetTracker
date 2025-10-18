namespace AssetTracker.WpfApp.Common.Events
{
    /// <summary>
    /// Used for tracking changes in processing url link in search for assets
    /// </summary>
    public class CheckLinkStatusChangedEvent
    {
        public string ServiceName { get; set; }
        public GenericProcessStatus CheckLinkProcessStatus { get; set; }
    }
}
