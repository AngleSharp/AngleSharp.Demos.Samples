namespace Samples
{
    using AngleSharp.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class EventAggregator : IEventAggregator
    {
        readonly List<Object> subscribers;

        public EventAggregator()
        {
            subscribers = new List<Object>();
        }

        public void Publish<TEvent>(TEvent data)
        {
            foreach (var subscriber in subscribers.OfType<ISubscriber<TEvent>>())
                subscriber.OnEventData(data);
        }

        public void Subscribe<TEvent>(ISubscriber<TEvent> listener)
        {
            subscribers.Add(listener);
        }

        public void Unsubscribe<TEvent>(ISubscriber<TEvent> listener)
        {
            subscribers.RemoveAll(m => m == listener);
        }
    }
}
