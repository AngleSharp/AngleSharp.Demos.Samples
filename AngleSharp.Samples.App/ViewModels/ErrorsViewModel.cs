namespace Samples.ViewModels
{
    using AngleSharp.Events;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    class ErrorsViewModel : BaseViewModel, IEventViewModel
    {
        readonly IEventAggregator _events;
        readonly ObservableCollection<CssParseErrorEvent> _cssErrors;
        readonly ObservableCollection<HtmlParseErrorEvent> _htmlErrors;

        public ErrorsViewModel(IEventAggregator events)
        {
            _events = events;
            _cssErrors = new ObservableCollection<CssParseErrorEvent>();
            _htmlErrors = new ObservableCollection<HtmlParseErrorEvent>();
            Register<CssParseErrorEvent>(m => App.Current.Dispatcher.Invoke(() => _cssErrors.Add(m)));
            Register<HtmlParseErrorEvent>(m => App.Current.Dispatcher.Invoke(() => _htmlErrors.Add(m)));
        }

        public IEnumerable<CssParseErrorEvent> Css
        {
            get { return _cssErrors; }
        }

        public IEnumerable<HtmlParseErrorEvent> Html
        {
            get { return _htmlErrors; }
        }

        void Register<T>(Action<T> listener)
        {
            var subscriber = new Subscriber<T>(listener);
            _events.Subscribe(subscriber);
        }

        public void Reset()
        {
            _cssErrors.Clear();
            _htmlErrors.Clear();
        }
    }
}
