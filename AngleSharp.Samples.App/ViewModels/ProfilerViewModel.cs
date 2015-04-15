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
        readonly Stopwatch _time;

        public ProfilerViewModel(IEventAggregator events)
        {
            _time = new Stopwatch();
            _items = new Collection<Item>();
            _events = events;
            Register<CssParseStartEvent>(m =>
            {
                var start = _time.Elapsed;
                m.Ended += _ => AddItem("Parse CSS " + _.Href, OxyColors.Violet, start, _time.Elapsed);
            });
            Register<HtmlParseStartEvent>(m =>
            {
                var start = _time.Elapsed;
                m.Ended += _ => AddItem("Parse HTML " + _.Url, OxyColors.Orange, start, _time.Elapsed);
            });
            Register<RequestStartEvent>(m =>
            {
                var start = _time.Elapsed;
                m.Ended += _ => AddItem("Response " + _.Address, OxyColors.Red, start, _time.Elapsed);
            });
        }

        void AddItem(String label, OxyColor color, TimeSpan start, TimeSpan end)
        {
            _items.Add(new Item { Label = label, Color = color, Start = start, End = end });
        }

        void Register<T>(Action<T> listener)
        {
            var subscriber = new Subscriber<T>(listener);
            _events.Subscribe(subscriber);
        }

        public void Reset()
        {
            _items.Clear();
            _time.Restart();
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

            public TimeSpan Start
            {
                get;
                set;
            }

            public TimeSpan End
            {
                get;
                set;
            }

            public Double Value
            {
                get { return End.TotalMilliseconds - Start.TotalMilliseconds; }
            }

            public OxyColor Color
            {
                get;
                set;
            }
        }
    }
}
