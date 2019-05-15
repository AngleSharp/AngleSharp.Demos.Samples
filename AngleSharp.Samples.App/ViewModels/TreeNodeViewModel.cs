namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using AngleSharp.Html;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    public class TreeNodeViewModel : BaseViewModel
    {
        private readonly ObservableCollection<TreeNodeViewModel> children;
        private Boolean expanded;
        private Boolean selected;
        private String value;
        private Brush foreground;
        private TreeNodeViewModel expansionElement;
        private ObservableCollection<TreeNodeViewModel> parent;

        private TreeNodeViewModel()
        {
            children = new ObservableCollection<TreeNodeViewModel>();
            expanded = false;
            selected = false;
            foreground = Brushes.Black;
        }

        public Boolean IsExpanded
        {
            get { return expanded; }
            set
            {
                expanded = value;
                RaisePropertyChanged();

                if (expansionElement != null && parent != null)
                {
                    if (value)
                        parent.Insert(parent.IndexOf(this) + 1, expansionElement);
                    else
                        parent.Remove(expansionElement);
                }
            }
        }

        public Brush Foreground
        {
            get { return foreground; }
            set { foreground = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<TreeNodeViewModel> Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        public Boolean IsSelected
        {
            get { return selected; }
            set
            {
                selected = value;
                RaisePropertyChanged();
            }
        }

        public String Value
        {
            get { return value; }
            set
            {
                this.value = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<TreeNodeViewModel> Children
        {
            get { return children; }
        }

        public static TreeNodeViewModel Create(INode node)
        {
            if (node is IText)
                return Create((IText)node);
            else if (node is IComment)
                return new TreeNodeViewModel { Value = HtmlMarkupFormatter.Instance.Comment((IComment)node), Foreground = Brushes.Gray };
            else if (node is IDocumentType)
                return new TreeNodeViewModel { Value = HtmlMarkupFormatter.Instance.Doctype((IDocumentType)node), Foreground = Brushes.DarkGray };
            else if(node is IElement)
                return Create((IElement)node);

            return null;
        }

        private static TreeNodeViewModel Create(IText text)
        {
            if(String.IsNullOrEmpty(text.Data))
                return null;

            var data = text.Data.Split(ws, StringSplitOptions.RemoveEmptyEntries);

            if (data.Length == 0)
                return null;

            return new TreeNodeViewModel { Value = HtmlMarkupFormatter.Instance.Text(text), Foreground = Brushes.SteelBlue };
        }

        private static TreeNodeViewModel Create(IElement node)
        {
            var vm = new TreeNodeViewModel { Value = OpenTag(node) };

            foreach (var element in SelectFrom(node.ChildNodes))
            {
                element.parent = vm.children;
                vm.children.Add(element);
            }

            if (vm.children.Count != 0)
                vm.expansionElement = new TreeNodeViewModel { Value = CloseTag(node) };

            return vm;
        }

        public static IEnumerable<TreeNodeViewModel> SelectFrom(IEnumerable<INode> nodes)
        {
            foreach (var node in nodes)
            {
                TreeNodeViewModel element = Create(node);

                if (element != null)
                    yield return element;
            }
        }

        private static String OpenTag(IElement element)
        {
            return HtmlMarkupFormatter.Instance.OpenTag(element, false);
        }

        private static String CloseTag(IElement element)
        {
            return HtmlMarkupFormatter.Instance.CloseTag(element, false);
        }
    }
}
