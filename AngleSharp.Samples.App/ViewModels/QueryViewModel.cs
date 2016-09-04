namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Windows.Media;

    public class QueryViewModel : BaseViewModel, ITabViewModel
    {
        private readonly ObservableCollection<IElement> _source;
        private String _query;
        private IDocument _document;
        private Brush _state;
        private Int32 _result;
        private Int64 _time;

        public QueryViewModel()
        {
            _state = Brushes.LightGray;
            _source = new ObservableCollection<IElement>();
            _query = "*";
        }

        public Int32 Result
        {
            get { return _result; }
            set
            {
                _result = value;
                RaisePropertyChanged();
            }
        }

        public Int64 Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged();
            }
        }

        public String Query
        {
            get { return _query; }
            set
            {
                _query = value;
                ChangeQuery();
                RaisePropertyChanged();
            }
        }

        public Brush State
        {
            get { return _state; }
            set
            {
                _state = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IElement> Source
        {
            get { return _source; }
        }

        private void ChangeQuery()
        {
            if (_document != null)
            {
                State = Brushes.LightGreen;

                try
                {
                    var sw = Stopwatch.StartNew();
                    var elements = _document.QuerySelectorAll(_query);
                    sw.Stop();
                    _source.Clear();

                    foreach (var element in elements)
                    {
                        _source.Add(element);
                    }

                    State = Brushes.White;
                    Time = sw.ElapsedMilliseconds;
                    Result = elements.Length;
                }
                catch (DomException)
                {
                    State = Brushes.LightPink;
                }
            }
        }

        public IDocument Document
        {
            get
            {
                return _document;
            }
            set
            {
                State = Brushes.LightGray;
                _document = value;
                ChangeQuery();
            }
        }
    }
}
