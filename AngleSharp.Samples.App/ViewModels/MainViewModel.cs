namespace Samples.ViewModels
{
    using AngleSharp;
    using AngleSharp.Network;
    using AngleSharp.Network.Default;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    class MainViewModel : BaseViewModel
    {
        #region Fields

        readonly ITabViewModel[] views;
        readonly IEventViewModel[] logs;
        readonly IBrowsingContext context;

        Task current;
        CancellationTokenSource cts;
        String status;
        String address;

        #endregion

        #region Child View Models

        readonly DOMViewModel dom;
        readonly ProfilerViewModel profiler;
        readonly QueryViewModel query;
        readonly ReplViewModel repl;
        readonly SettingsViewModel settings;
        readonly StatisticsViewModel statistics;
        readonly TreeViewModel tree;
        readonly SheetViewModel sheets;
        readonly ErrorsViewModel errors;

        #endregion

        #region ctor

        public MainViewModel()
        {
            var events = new EventAggregator();
            var requester = new IRequester[] { new DataRequester(), new HttpRequester() };
            var config = new Configuration(events: events).WithCss().WithDefaultLoader(m => 
            {
                m.IsNavigationEnabled = true;
                m.IsResourceLoadingEnabled = true;
            }, requester);
            context = BrowsingContext.New(config);
            profiler = new ProfilerViewModel(events);
            errors = new ErrorsViewModel(events);
            dom = new DOMViewModel();
            query = new QueryViewModel();
            repl = new ReplViewModel();
            settings = new SettingsViewModel();
            statistics = new StatisticsViewModel();
            tree = new TreeViewModel();
            sheets = new SheetViewModel();
            cts = new CancellationTokenSource();
            views = new ITabViewModel[] 
            {
                dom,
                query,
                repl,
                statistics,
                tree,
                sheets
            };
            logs = new IEventViewModel[]
            {
                profiler,
                errors
            };
        }

        #endregion

        #region Properties

        public DOMViewModel Dom
        {
            get { return dom; }
        }

        public ErrorsViewModel Errors
        {
            get { return errors; }
        }

        public ProfilerViewModel Profiler
        {
            get { return profiler; }
        }

        public QueryViewModel Queries
        {
            get { return query; }
        }

        public ReplViewModel Console
        {
            get { return repl; }
        }

        public SettingsViewModel Settings
        {
            get { return settings; }
        }

        public StatisticsViewModel Statistics
        {
            get { return statistics; }
        }

        public TreeViewModel Tree
        {
            get { return tree; }
        }

        public SheetViewModel Sheets
        {
            get { return sheets; }
        }

        public String Address
        {
            get { return address; }
            set 
            { 
                address = value; 
                RaisePropertyChanged();
            }
        }

        public String Status
        {
            get { return status; }
            set
            {
                status = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Methods

        public void Go()
        {
            var url = CreateUrlFrom(address);

            if (current != null && !current.IsCompleted)
            {
                cts.Cancel();
                cts = new CancellationTokenSource();
            }

            profiler.Reset();
            current = LoadAsync(url, cts.Token);
        }

        async Task LoadAsync(Url url, CancellationToken cancel)
        {
            Status = String.Format("Loading {0} ...", url.Href);

            foreach (var log in logs)
                log.Reset();

            var document = await context.OpenAsync(url, cancel);

            foreach (var view in views)
                view.Document = document;

            Status = String.Format("Loaded {0}.", url.Href);
        }

        #endregion
    }
}
