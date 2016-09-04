namespace Samples.ViewModels
{
    using AngleSharp;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    sealed class MainViewModel : BaseViewModel
    {
        #region Fields

        private readonly ITabViewModel[] _views;
        private readonly IEventViewModel[] _logs;
        private readonly IBrowsingContext _context;

        private Task _current;
        private CancellationTokenSource _cts;
        private String _status;
        private String _address;

        #endregion

        #region Child View Models

        private readonly DOMViewModel _dom;
        private readonly ProfilerViewModel _profiler;
        private readonly QueryViewModel _query;
        private readonly ReplViewModel _repl;
        private readonly SettingsViewModel _settings;
        private readonly StatisticsViewModel _statistics;
        private readonly TreeViewModel _tree;
        private readonly SheetViewModel _sheets;
        private readonly ErrorsViewModel _errors;

        #endregion

        #region ctor

        public MainViewModel()
        {
            var config = Configuration.Default.WithCss().WithRequesters(setup => 
            {
                setup.IsNavigationEnabled = true;
                setup.IsResourceLoadingEnabled = true;
            });
            _context = BrowsingContext.New(config);
            _profiler = new ProfilerViewModel(_context);
            _errors = new ErrorsViewModel(_context);
            _dom = new DOMViewModel();
            _query = new QueryViewModel();
            _repl = new ReplViewModel();
            _settings = new SettingsViewModel();
            _statistics = new StatisticsViewModel();
            _tree = new TreeViewModel();
            _sheets = new SheetViewModel();
            _cts = new CancellationTokenSource();
            _views = new ITabViewModel[] 
            {
                _dom,
                _query,
                _repl,
                _statistics,
                _tree,
                _sheets
            };
            _logs = new IEventViewModel[]
            {
                _profiler,
                _errors
            };
        }

        #endregion

        #region Properties

        public DOMViewModel Dom
        {
            get { return _dom; }
        }

        public ErrorsViewModel Errors
        {
            get { return _errors; }
        }

        public ProfilerViewModel Profiler
        {
            get { return _profiler; }
        }

        public QueryViewModel Queries
        {
            get { return _query; }
        }

        public ReplViewModel Console
        {
            get { return _repl; }
        }

        public SettingsViewModel Settings
        {
            get { return _settings; }
        }

        public StatisticsViewModel Statistics
        {
            get { return _statistics; }
        }

        public TreeViewModel Tree
        {
            get { return _tree; }
        }

        public SheetViewModel Sheets
        {
            get { return _sheets; }
        }

        public String Address
        {
            get { return _address; }
            set 
            { 
                _address = value; 
                RaisePropertyChanged();
            }
        }

        public String Status
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        public void Go()
        {
            var url = CreateUrlFrom(_address);

            if (_current != null && !_current.IsCompleted)
            {
                _cts.Cancel();
                _cts = new CancellationTokenSource();
            }

            _profiler.Reset();
            _current = LoadAsync(url, _cts.Token);
        }

        private async Task LoadAsync(Url url, CancellationToken cancel)
        {
            Status = String.Format("Loading {0} ...", url.Href);

            foreach (var log in _logs)
            {
                log.Reset();
            }

            var document = await _context.OpenAsync(url, cancel);

            foreach (var view in _views)
            {
                view.Document = document;
            }

            Status = String.Format("Loaded {0}.", url.Href);
        }

        #endregion
    }
}
