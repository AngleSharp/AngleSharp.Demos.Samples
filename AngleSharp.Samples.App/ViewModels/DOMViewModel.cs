namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using System.Collections.ObjectModel;

    public class DOMViewModel : BaseViewModel, ITabViewModel
    {
        ObservableCollection<DOMNodeViewModel> source;
        IDocument document;

        public DOMViewModel ()
	    {
            source = new ObservableCollection<DOMNodeViewModel>();
	    }

        public ObservableCollection<DOMNodeViewModel> Source
        {
            get { return source; }
        }

        public DOMNodeViewModel Root
        {
            set
            {
                source.Clear();
                source.Add(value);
            }
        }

        public IDocument Document
        {
            get
            {
                return document;
            }
            set
            {
                document = value;
                Root = new DOMNodeViewModel(document);
            }
        }
    }
}
