namespace Samples.ViewModels
{
    using AngleSharp.Events;
    using OxyPlot;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    public class ProfilerViewModel : BaseViewModel
    {
        readonly Collection<Item> _items;
        readonly IEventAggregator _events;

        public ProfilerViewModel(IEventAggregator events)
        {
            _items = new Collection<Item>();
            _events = events;
            Register<CssParseStartEvent>(m => 
            {
                var sw = Stopwatch.StartNew();
                m.Ended += _ => AddItem("Parse CSS", OxyColors.Violet, sw);
            });
            Register<HtmlParseStartEvent>(m =>
            {
                var sw = Stopwatch.StartNew();
                m.Ended += _ => AddItem("Parse HTML", OxyColors.Orange, sw);
            });
            Register<RequestStartEvent>(m =>
            {
                var sw = Stopwatch.StartNew();
                m.Ended += _ => AddItem("Response", OxyColors.Red, sw);
            });
        }

        void AddItem(String label, OxyColor color, Stopwatch watch)
        {
            watch.Stop();
            _items.Add(new Item { Label = label, Color = color, Time = watch.Elapsed });
        }

        void Register<T>(Action<T> listener)
        {
            var subscriber = new Subscriber<T>(listener);
            _events.Subscribe(subscriber);
        }

        public Collection<Item> Items
        {
            get { return _items; }
        }

        public class Item
        {
            public String Label
            {
                get;
                set;
            }

            public TimeSpan Time
            {
                get;
                set;
            }

            public Double Value
            {
                get { return Time.Milliseconds; }
            }

            public OxyColor Color
            {
                get;
                set;
            }
        }
    }
}
