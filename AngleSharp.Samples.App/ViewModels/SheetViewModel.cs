namespace Samples.ViewModels
{
    using AngleSharp;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Html;
    using System;
    using System.Collections.ObjectModel;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class SheetViewModel : BaseViewModel, ITabViewModel
    {
        ObservableCollection<IElement> source;
        IElement selected;
        ObservableCollection<CssRuleViewModel> tree;
        CancellationTokenSource cts;
        Task populate;
        IDocument document;

        public SheetViewModel()
	    {
            source = new ObservableCollection<IElement>();
            tree = new ObservableCollection<CssRuleViewModel>();
            cts = new CancellationTokenSource();
	    }

        public ObservableCollection<IElement> Source
        {
            get { return source; }
        }

        public ObservableCollection<CssRuleViewModel> Tree
        {
            get { return tree; }
        }

        public IElement Selected
        {
            get { return selected; }
            set 
            {
                selected = value;     
                RaisePropertyChanged();

                if (populate != null && !populate.IsCompleted)
                {
                    cts.Cancel();
                    cts = new CancellationTokenSource();
                }

                populate = PopulateTree(cts.Token);
            }
        }

        async Task PopulateTree(CancellationToken token)
        {
            tree.Clear();
            var content = String.Empty;

            if (selected is IHtmlLinkElement)
            {
                var url = new Uri(document.DocumentUri);
                var http = new HttpClient { BaseAddress = url };
                var request = await http.GetAsync(((IHtmlLinkElement)selected).Href, cts.Token);
                content = await request.Content.ReadAsStringAsync();
                token.ThrowIfCancellationRequested();
            }
            else if (selected is IHtmlStyleElement)
                content = ((IHtmlStyleElement)selected).TextContent;
            
            var css = DocumentBuilder.Css(content);

            for (int i = 0; i < css.Rules.Length; i++)
                tree.Add(new CssRuleViewModel(css.Rules[i]));
        }

        public IDocument Document
        {
            get { return document; }
            set
            {
                document = value;
                source.Clear();

                foreach (var sheet in document.QuerySelectorAll("link,style"))
                    source.Add(sheet);

                Selected = null;
            }
        }
    }
}
