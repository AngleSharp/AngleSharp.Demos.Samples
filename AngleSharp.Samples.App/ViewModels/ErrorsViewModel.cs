namespace Samples.ViewModels
{
    using AngleSharp;
    using AngleSharp.Css.Dom.Events;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Html.Dom.Events;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    class ErrorsViewModel : BaseViewModel, IEventViewModel
    {
        private readonly IBrowsingContext _context;
        private readonly ObservableCollection<CssErrorEvent> _cssErrors;
        private readonly ObservableCollection<HtmlErrorEvent> _htmlErrors;

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

        private void Register<T>(Action<T> listener)
            where T : Event
        {
            _context.AddEventListener(EventNames.Error, (obj, ev) =>
            {
                if (ev is T data)
                {
                    listener.Invoke(data);
                }
            });
        }

        public void Reset()
        {
            _cssErrors.Clear();
            _htmlErrors.Clear();
        }
    }
}
