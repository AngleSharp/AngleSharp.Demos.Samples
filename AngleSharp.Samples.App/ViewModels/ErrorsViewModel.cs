namespace Samples.ViewModels
{
    using AngleSharp;
    using AngleSharp.Dom.Events;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    class ErrorsViewModel : BaseViewModel, IEventViewModel
    {
        readonly IBrowsingContext _context;
        readonly ObservableCollection<CssErrorEvent> _cssErrors;
        readonly ObservableCollection<HtmlErrorEvent> _htmlErrors;

        public ErrorsViewModel(IBrowsingContext context)
        {
            _context = context;
            _cssErrors = new ObservableCollection<CssErrorEvent>();
            _htmlErrors = new ObservableCollection<HtmlErrorEvent>();
            Register<CssErrorEvent>(m => App.Current.Dispatcher.Invoke(() => _cssErrors.Add(m)));
            Register<HtmlErrorEvent>(m => App.Current.Dispatcher.Invoke(() => _htmlErrors.Add(m)));
        }

        public IEnumerable<CssErrorEvent> Css
        {
            get { return _cssErrors; }
        }

        public IEnumerable<HtmlErrorEvent> Html
        {
            get { return _htmlErrors; }
        }

        void Register<T>(Action<T> listener)
            where T : Event
        {
            _context.ParseError += (obj, ev) =>
            {
                var data = ev as T;

                if (data != null)
                {
                    listener.Invoke(data);
                }
            };
        }

        public void Reset()
        {
            _cssErrors.Clear();
            _htmlErrors.Clear();
        }
    }
}
