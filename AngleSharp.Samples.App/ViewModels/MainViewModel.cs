namespace Samples.ViewModels
{
    using AngleSharp;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    class MainViewModel : BaseViewModel
    {
        #region Fields

        Task current;
        CancellationTokenSource cts;
        String status;
        String address;
        ITabViewModel[] views;

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

        #endregion

        #region ctor

        public MainViewModel()
        {
            dom = new DOMViewModel();
            profiler = new ProfilerViewModel();
            query = new QueryViewModel();
            repl = new ReplViewModel();
            settings = new SettingsViewModel();
            statistics = new StatisticsViewModel();
            tree = new TreeViewModel();
            sheets = new SheetViewModel();
            cts = new CancellationTokenSource();
            views = new ITabViewModel[] {
                dom,
                query,
                repl,
                statistics,
                tree,
                sheets
            };
        }

        #endregion

        #region Properties

        public DOMViewModel Dom
        {
            get { return dom; }
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
            if (current != null && !current.IsCompleted)
            {
                cts.Cancel();
                cts = new CancellationTokenSource();
            }

            current = LoadAsync(address, cts.Token);
        }

        async Task LoadAsync(String url, CancellationToken cancel)
        {
            var response = default(Stream);
            var http = new HttpClient();
            var uri = Sanitize(url);
            Status = "Loading " + uri.AbsoluteUri + " ...";

            if (uri.Scheme.Equals("file", StringComparison.Ordinal))
            {
                //ProfilerViewModel.Data.Start("Response (HTML)", OxyPlot.OxyColors.Red);
                response = File.Open(uri.AbsolutePath.Substring(1), FileMode.Open);
                //ProfilerViewModel.Data.Stop();
            }
            else
            {
                //ProfilerViewModel.Data.Start("Response (HTML)", OxyPlot.OxyColors.Red);
                var request = await http.GetAsync(uri, cancel);
                response = await request.Content.ReadAsStreamAsync();
                //ProfilerViewModel.Data.Stop();
                cancel.ThrowIfCancellationRequested();
            }

            Status = "Parsing " + uri.AbsoluteUri + " ...";
            //ProfilerViewModel.Data.Start("Parsing (HTML)", OxyPlot.OxyColors.Orange);
            var document = DocumentBuilder.Html(response, new Configuration(), uri.AbsoluteUri);
            //ProfilerViewModel.Data.Stop();
            response.Close();

            cancel.ThrowIfCancellationRequested();

            foreach (var view in views)
                view.Document = document;

            Status = "Displaying: " + url;
        }

        #endregion
    }
}
