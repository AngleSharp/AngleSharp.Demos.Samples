﻿namespace Samples.ViewModels
{
    using AngleSharp.Css.Dom;
    using AngleSharp.Css.Dom.Events;
    using AngleSharp.Dom;
    using AngleSharp.Dom.Events;
    using System;
    using System.Collections.ObjectModel;

    public class SheetViewModel : BaseViewModel, ITabViewModel
    {
        private readonly ObservableCollection<IStyleSheet> _source;
        private readonly ObservableCollection<CssRuleViewModel> _tree;
        private IStyleSheet _selected;
        private IDocument _document;

        public SheetViewModel()
	    {
            _source = new ObservableCollection<IStyleSheet>();
            _tree = new ObservableCollection<CssRuleViewModel>();
	    }

        public ObservableCollection<IStyleSheet> Source
        {
            get { return _source; }
        }

        public ObservableCollection<CssRuleViewModel> Tree
        {
            get { return _tree; }
        }

        public IStyleSheet Selected
        {
            get { return _selected; }
            set 
            {
                _selected = value;
                RaisePropertyChanged();
                _tree.Clear();

                if (_selected is ICssStyleSheet sheet)
                {
                    for (int i = 0; i < sheet.Rules.Length; i++)
                    {
                        var rule = new CssRuleViewModel(sheet.Rules[i]);
                        _tree.Add(rule);
                    }
                }
            }
        }

        public IDocument Document
        {
            get { return _document; }
            set
            {
                if (_document != null)
                {
                    _document.Context.RemoveEventListener(EventNames.Parsed, ParseEnded);
                }

                _document = value;
                _source.Clear();

                foreach (var sheet in _document.StyleSheets)
                {
                    _source.Add(sheet);
                }

                _document.Context.AddEventListener(EventNames.Parsed, ParseEnded);
                Selected = null;
            }
        }

        private void ParseEnded(Object sender, Event ev)
        {
            if (ev is CssParseEvent data)
            {
                App.Current.Dispatcher.Invoke(() => _source.Add(data.StyleSheet));
            }
        }
    }
}
