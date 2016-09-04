namespace Samples.ViewModels
{
    using AngleSharp.Dom.Css;
    using System;
    using System.Collections.ObjectModel;

    public class CssRuleViewModel : BaseViewModel
    {
        private readonly ObservableCollection<CssRuleViewModel> _children;
        private readonly String _typeName;
        private readonly String _name;

        private CssRuleViewModel()
        {
            _children = new ObservableCollection<CssRuleViewModel>();
        }

        private CssRuleViewModel(Type type)
            : this()
        {
            _typeName = type.Name;
        }

        private CssRuleViewModel(Object o)
            : this(o.GetType())
        {
        }

        private CssRuleViewModel(String name, String value)
            : this()
        {
            _name = name;
            _typeName = "CssProperty";
            _children.Add(new CssRuleViewModel(value));
        }

        public CssRuleViewModel(ICssRule rule)
            : this(rule.GetType())
        {
            switch (rule.Type)
            {
                case CssRuleType.FontFace:
                    var font = (ICssFontFaceRule)rule;
                    _name = "@font-face";
                    Populate(font);
                    break;

                case CssRuleType.Keyframe:
                    var keyframe = (ICssKeyframeRule)rule;
                    _name = keyframe.KeyText;
                    Populate(keyframe.Style);
                    break;

                case CssRuleType.Keyframes:
                    var keyframes = (ICssKeyframesRule)rule;
                    _name = "@keyframes " + keyframes.Name;
                    Populate(keyframes.Rules);
                    break;

                case CssRuleType.Media:
                    var media = (ICssMediaRule)rule;
                    _name = "@media " + media.Media.MediaText;
                    Populate(media.Rules);
                    break;

                case CssRuleType.Page:
                    var page = (ICssPageRule)rule;
                    _name = "@page " + page.SelectorText;
                    Populate(page.Style);
                    break;

                case CssRuleType.Style:
                    var style = (ICssStyleRule)rule;
                    _name = style.SelectorText;
                    Populate(style.Style);
                    break;

                case CssRuleType.Supports:
                    var support = (ICssSupportsRule)rule;
                    _name = "@supports " + support.ConditionText;
                    Populate(support.Rules);
                    break;

                default:
                    _name = rule.CssText;
                    break;
            }
        }

        public CssRuleViewModel(ICssProperty declaration)
            : this(declaration.Name, declaration.Value)
        {
        }

        public CssRuleViewModel(String value)
            : this()
        {
            _name = value;
            _typeName = "CssValue";
        }

        private void Populate(ICssFontFaceRule font)
        {
            AddIfNotEmpty("Family", font.Family);
            AddIfNotEmpty("Features", font.Features);
            AddIfNotEmpty("Range", font.Range);
            AddIfNotEmpty("Source", font.Source);
            AddIfNotEmpty("Stretch", font.Stretch);
            AddIfNotEmpty("Style", font.Style);
            AddIfNotEmpty("Variant", font.Variant);
            AddIfNotEmpty("Weight", font.Weight);
        }

        private void AddIfNotEmpty(String name, String value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _children.Add(new CssRuleViewModel(name, value));
            }
        }

        private void Populate(ICssStyleDeclaration declarations)
        {
            foreach (var declaration in declarations)
            {
                _children.Add(new CssRuleViewModel(declaration));
            }
        }

        private void Populate(ICssRuleList rules)
        {
            foreach (var rule in rules)
            {
                _children.Add(new CssRuleViewModel(rule));
            }
        }

        public String Name
        {
            get { return _name; }
        }

        public String TypeName
        {
            get { return _typeName; }
        }

        public ObservableCollection<CssRuleViewModel> Children
        {
            get { return _children; }
        }
    }
}
