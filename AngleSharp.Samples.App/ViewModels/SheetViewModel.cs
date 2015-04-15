namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using AngleSharp.Dom.Css;
    using System.Collections.ObjectModel;

    public class SheetViewModel : BaseViewModel, ITabViewModel
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
                        tree.Add(new CssRuleViewModel(sheet.Rules[i]));
                }
            }
        }

        public IDocument Document
        {
            get { return document; }
            set
            {
                document = value;
                source.Clear();

                foreach (var sheet in document.StyleSheets)
                    source.Add(sheet);

                Selected = null;
            }
        }
    }
}
