namespace Samples.ViewModels
{
    using AngleSharp;
    using AngleSharp.Css.Dom.Events;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using AngleSharp.Html.Dom.Events;
    using OxyPlot;
    using OxyPlot.Axes;
    using OxyPlot.Series;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class ProfilerViewModel : BaseViewModel, IEventViewModel
    {
        private readonly Stopwatch _time;
        private readonly Dictionary<Object, TimeSpan> _tracker;
        private readonly IBrowsingContext _context;
        private readonly PlotModel _model;

        public ProfilerViewModel(IBrowsingContext context)
        {
            _time = new Stopwatch();
            _tracker = new Dictionary<Object, TimeSpan>();
            _context = context;
            _model = CreateModel();
            _context.AddEventListener(EventNames.Parsing, TrackParsing);
            _context.AddEventListener(EventNames.Parsed, TrackParsed);
            _context.AddEventListener(EventNames.Requesting, TrackRequesting);
            _context.AddEventListener(EventNames.Requested, TrackRequested);
        }

        private static PlotModel CreateModel()
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

        public PlotModel PlotData
        {
            get { return _model; }
        }

        public void Reset()
        {
            ((IntervalBarSeries)_model.Series[0]).Items.Clear();
            ((CategoryAxis)_model.Axes[0]).Labels.Clear();
            _time.Restart();
        }

        private void AddItem(String label, OxyColor color, TimeSpan start, TimeSpan end)
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

        private void TrackRequesting(Object sender, Event ev)
        {
            var data = ev as RequestEvent;

            if (data != null)
            {
                _tracker.Add(data.Request, _time.Elapsed);
            }
        }

        private void TrackRequested(Object sender, Event ev)
        {
            var data = ev as RequestEvent;
            var start = default(TimeSpan);

            if (data != null && _tracker.TryGetValue(data.Request, out start))
            {
                var request = data.Request;
                AddItem("Request for " + request.Address.Href, OxyColors.Red, start, _time.Elapsed);
                _tracker.Remove(request);
            }
        }

        private void TrackParsing(Object sender, Event ev)
        {
            if (ev is HtmlParseEvent html)
            {
                _tracker.Add(html.Document, _time.Elapsed);
            }
            else if (ev is CssParseEvent css)
            {
                _tracker.Add(css.StyleSheet, _time.Elapsed);
            }
        }

        private void TrackParsed(Object sender, Event ev)
        {
            if (ev is HtmlParseEvent html && _tracker.TryGetValue(html.Document, out var startHtml))
            {
                var document = html.Document;
                AddItem("Parse HTML " + document.Url, OxyColors.Orange, startHtml, _time.Elapsed);
                _tracker.Remove(document);
            }
            else if (ev is CssParseEvent css && _tracker.TryGetValue(css.StyleSheet, out var startCss))
            {
                var styleSheet = css.StyleSheet;
                AddItem("Parse CSS " + styleSheet.Href, OxyColors.Violet, startCss, _time.Elapsed);
                _tracker.Remove(styleSheet);
            }
        }
    }
}
