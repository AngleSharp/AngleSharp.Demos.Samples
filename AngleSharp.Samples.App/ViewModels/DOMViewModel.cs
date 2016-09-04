namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using System.Collections.ObjectModel;

    public class DOMViewModel : BaseViewModel, ITabViewModel
    {
        private readonly ObservableCollection<DOMNodeViewModel> _source;
        private IDocument _document;

        public DOMViewModel ()
	    {
            _source = new ObservableCollection<DOMNodeViewModel>();
	    }

        public ObservableCollection<DOMNodeViewModel> Source
        {
            get { return _source; }
        }

        public DOMNodeViewModel Root
        {
            set
            {
                _source.Clear();
                _source.Add(value);
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
                _document = value;
                Root = new DOMNodeViewModel(_document);
            }
        }
    }
}
