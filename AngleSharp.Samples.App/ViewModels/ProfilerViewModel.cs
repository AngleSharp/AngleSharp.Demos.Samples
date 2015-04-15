namespace Samples.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Linq;

    public class ProfilerViewModel : BaseViewModel
    {
        Collection<Item> _items;
        Stopwatch _sw;
        Item _tmp;

        public ProfilerViewModel()
        {
            _sw = new Stopwatch();
            _items = new Collection<Item>();
        }

        public Collection<Item> Items
        {
            get { return _items; }
        }

        public void Start(String label, OxyPlot.OxyColor color)
        {
            _tmp = new Item { Label = label, Color = color };
            _sw.Restart();
        }

        public void Stop()
        {
            _sw.Stop();
            _tmp.Value = (Double)_sw.ElapsedMilliseconds;
            var item = _items.Where(m => m.Label == _tmp.Label).SingleOrDefault();

            if (item != null)
                _items.Remove(item);

            _items.Add(_tmp);
        }

        public class Item
        {
            public String Label
            {
                get;
                set;
            }

            public Double Value
            {
                get;
                set;
            }

            public OxyPlot.OxyColor Color
            {
                get;
                set;
            }
        }
    }
}
