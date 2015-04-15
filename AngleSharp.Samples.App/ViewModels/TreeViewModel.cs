namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using System.Collections.ObjectModel;

    public class TreeViewModel : BaseViewModel, ITabViewModel
    {
        readonly ObservableCollection<TreeNodeViewModel> nodes;
        IDocument document;

        public TreeViewModel()
        {
            nodes = new ObservableCollection<TreeNodeViewModel>();
        }

        public ObservableCollection<TreeNodeViewModel> Tree
        {
            get { return nodes; }
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
                nodes.Clear();
                var elements = TreeNodeViewModel.SelectFrom(document.ChildNodes);

                foreach (var element in elements)
                {
                    element.Parent = nodes;
                    nodes.Add(element);
                }
            }
        }
    }
}
