namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Css;
    using AngleSharp.Dom.Html;
    using AngleSharp;
    using System.Collections.ObjectModel;
    using AngleSharp.Events;
    using System.Windows.Threading;

    public class SheetViewModel : BaseViewModel, ITabViewModel, ISubscriber<CssParseStartEvent>
    {
        readonly ObservableCollection<IStyleSheet> source;
        readonly ObservableCollection<CssRuleViewModel> tree;
        IStyleSheet selected;
        IDocument document;

        public SheetViewModel()
	    {
            source = new ObservableCollection<IStyleSheet>();
            tree = new ObservableCollection<CssRuleViewModel>();
	    }

        public ObservableCollection<IStyleSheet> Source
        {
            get { return source; }
        }

        public ObservableCollection<CssRuleViewModel> Tree
        {
            get { return tree; }
        }

        public IStyleSheet Selected
        {
            get { return selected; }
            set 
            {
                selected = value;
                RaisePropertyChanged();
                tree.Clear();
                var sheet = selected as ICssStyleSheet;

                if (sheet != null)
                {
                    for (int i = 0; i < sheet.Rules.Length; i++)
                    {
                        var rule = new CssRuleViewModel(sheet.Rules[i]);
                        tree.Add(rule);
                    }
                }
            }
        }

        public IDocument Document
        {
            get { return document; }
            set
            {
                if (document != null)
                    document.Context.Configuration.Events.Unsubscribe(this);

                document = value;
                source.Clear();

                foreach (var sheet in document.StyleSheets)
                    source.Add(sheet);

                document.Context.Configuration.Events.Subscribe(this);
                Selected = null;
            }
        }

        void ISubscriber<CssParseStartEvent>.OnEventData(CssParseStartEvent data)
        {
            data.Ended += sheet =>
            {
                App.Current.Dispatcher.Invoke(() => source.Add(sheet));
            };
        }
    }
}
