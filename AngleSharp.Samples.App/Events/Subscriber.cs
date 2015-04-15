namespace Samples
{
    using AngleSharp.Events;
    using System;

    class Subscriber<T> : ISubscriber<T>
    {
        readonly Action<T> _listener;

        public Subscriber(Action<T> listener)
        {
            _listener = listener;
        }

        public void OnEventData(T data)
        {
            _listener(data);
        }
    }
}
