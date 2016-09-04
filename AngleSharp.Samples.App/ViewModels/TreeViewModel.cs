namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using System.Collections.ObjectModel;

    public class TreeViewModel : BaseViewModel, ITabViewModel
    {
        private readonly ObservableCollection<TreeNodeViewModel> _nodes;
        private IDocument _document;

        public TreeViewModel()
        {
            _nodes = new ObservableCollection<TreeNodeViewModel>();
        }

        public ObservableCollection<TreeNodeViewModel> Tree
        {
            get { return _nodes; }
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
                _nodes.Clear();
                var elements = TreeNodeViewModel.SelectFrom(_document.ChildNodes);

                foreach (var element in elements)
                {
                    element.Parent = _nodes;
                    _nodes.Add(element);
                }
            }
        }
    }
}
