namespace Samples.ViewModels
{
    using AngleSharp.Events;
    using System;

    class ErrorsViewModel : BaseViewModel, IEventViewModel
    {
        readonly IEventAggregator _events;

        public ErrorsViewModel(IEventAggregator events)
        {
            _events = events;
            Register<CssParseErrorEvent>(m =>
            {
                
            });
            Register<HtmlParseErrorEvent>(m =>
            {

            });
        }

        void Register<T>(Action<T> listener)
        {
            var subscriber = new Subscriber<T>(listener);
            _events.Subscribe(subscriber);
        }

        public void Reset()
        {
            //throw new NotImplementedException();
        }
    }
}
