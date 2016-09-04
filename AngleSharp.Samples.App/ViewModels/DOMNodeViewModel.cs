namespace Samples.ViewModels
{
    using AngleSharp.Attributes;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;

    public class DOMNodeViewModel : BaseViewModel
    {
        private readonly ObservableCollection<DOMNodeViewModel> _children;
        private readonly String _name;
        private readonly Object _element;
        private readonly DOMNodeViewModel _parent;
        private String _typeName;
        private String _value;
        private Boolean _selected;
        private Boolean _expanded;
        private Boolean _populated;

        public DOMNodeViewModel(Object nodeElement, String nodeName = "document", DOMNodeViewModel nodeParent = null)
        {
            _element = nodeElement;
            _parent = nodeParent;
            _children = new ObservableCollection<DOMNodeViewModel>();
            _name = nodeName;

            if (nodeElement == null)
            {
                _populated = true;
                _typeName = "<null>";
            }
            else if (nodeParent == null)
            {
                CreateChildren();
                IsExpanded = true;
                IsSelected = true;
            }
        }

        public String Name
        {
            get { return _name; }
        }

        public String Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged();
            }
        }

        public DOMNodeViewModel Parent
        {
            get { return _parent; }
        }

        public String TypeName
        {
            get { return _typeName; }
        }

        public ObservableCollection<DOMNodeViewModel> Children
        {
            get { return _children; }
        }

        public Boolean IsSelected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                RaisePropertyChanged();
            }
        }

        public Boolean IsExpanded
        {
            get { return _expanded; }
            set
            {
                _expanded = value;

                foreach (var child in _children)
                    child.CreateChildren();

                RaisePropertyChanged();
            }
        }

        private void CreateChildren()
        {
            if (!_populated)
            {
                var type = _element.GetType();
                _typeName = type.Name;
                SetMembers(type);
                _populated = true;
            }
        }

        private void SetMembers(Type type)
        {
            if (type.GetCustomAttribute<DomNameAttribute>() == null)
            {
                foreach (var contract in type.GetInterfaces())
                {
                    SetMembers(contract);
                }
            }
            else
            {
                SetProperties(type.GetProperties());
            }
        }

        private void SetProperties(IEnumerable<PropertyInfo> properties)
        {
            var hv = true;

            foreach (var property in properties)
            {
                var names = property.GetCustomAttributes<DomNameAttribute>();

                foreach (var name in names.Select(m => m.OfficialName))
                {
                    hv = false;

                    switch (property.GetIndexParameters().Length)
                    {
                        case 0:
                        {
                            var value = property.GetValue(_element);
                            _children.Add(new DOMNodeViewModel(value, name, this));
                            break;
                        }
                        case 1:
                        {
                            if (_element is IEnumerable)
                            {
                                var collection = (IEnumerable)_element;
                                var index = 0;
                                var idx = new object[1];

                                foreach (var item in collection)
                                {
                                    idx[0] = index;
                                    _children.Add(new DOMNodeViewModel(item, "[" + index.ToString() + "]", this));
                                    index++;
                                }
                            }
                            break;
                        }
                    }
                }
            }

            if (hv)
            {
                Value = _element.ToString();
            }
        }
    }
}
