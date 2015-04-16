namespace Samples.ViewModels
{
    using AngleSharp.Events;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System;
    using System.Diagnostics;

    public class ProfilerViewModel : BaseViewModel, IEventViewModel
    {
        readonly IEventAggregator _events;
        readonly Stopwatch _time;
        readonly PlotModel _model;

        public ProfilerViewModel(IEventAggregator events)
        {
            _time = new Stopwatch();
            _events = events;
            _model = CreateModel();
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
                m.Ended += _ => AddItem((_ != null ? "Response " + _.Address.Href : "No response"), OxyColors.Red, start, _time.Elapsed);
            });
        }

        static PlotModel CreateModel()
        {
            var model = new PlotModel { LegendPlacement = LegendPlacement.Outside };
            var valueAxis = new LinearAxis { Position = AxisPosition.Bottom, MinimumPadding = 0.1, MaximumPadding = 0.1 };
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Left };
            var series = new IntervalBarSeries();
            model.Axes.Add(categoryAxis);
            model.Axes.Add(valueAxis);
            model.Series.Add(series);
            return model;
        }

        void AddItem(String label, OxyColor color, TimeSpan start, TimeSpan end)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                var series = _model.Series[0] as IntervalBarSeries;
                var category = _model.Axes[0] as CategoryAxis;
                category.Labels.Add((category.Labels.Count + 1).ToString());
                var begin = start.TotalMilliseconds;
                var final = end.TotalMilliseconds;
                series.Items.Add(new IntervalBarItem(begin, final, label) { Color = color });
                _model.InvalidatePlot(true);
            });
        }

        void Register<T>(Action<T> listener)
        {
            var subscriber = new Subscriber<T>(listener);
            _events.Subscribe(subscriber);
        }

        public PlotModel PlotData
        {
            get { return _model; }
        }

        public void Reset()
        {
            (_model.Series[0] as IntervalBarSeries).Items.Clear();
            (_model.Axes[0] as CategoryAxis).Labels.Clear();
            _time.Restart();
        }
    }
}
