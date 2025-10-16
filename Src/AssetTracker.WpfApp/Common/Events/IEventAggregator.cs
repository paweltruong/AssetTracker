namespace AssetTracker.WpfApp.Common.Events
{
    public interface IEventAggregator
    {
        void Subscribe<TEvent>(Action<TEvent> handler);

        void Publish<TEvent>(TEvent eventToPublish);
    }
}
