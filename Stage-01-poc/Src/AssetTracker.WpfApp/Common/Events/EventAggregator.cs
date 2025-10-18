namespace AssetTracker.WpfApp.Common.Events
{
    public class EventAggregator : IEventAggregator
    {
        private readonly Dictionary<Type, List<object>> _handlers = new();

        public void Subscribe<TEvent>(Action<TEvent> handler)
        {
            var eventType = typeof(TEvent);
            if (!_handlers.ContainsKey(eventType))
                _handlers[eventType] = new List<object>();

            _handlers[eventType].Add(handler);
        }

        public void Publish<TEvent>(TEvent eventToPublish)
        {
            var eventType = typeof(TEvent);
            if (_handlers.ContainsKey(eventType))
            {
                foreach (var handler in _handlers[eventType])
                {
                    ((Action<TEvent>)handler)(eventToPublish);
                }
            }
        }
    }
}
