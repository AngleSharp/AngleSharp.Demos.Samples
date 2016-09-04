namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using AngleSharp.Scripting.JavaScript;
    using Jint.Runtime;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    sealed class ReplViewModel : BaseViewModel, ITabViewModel
    {
        private readonly String _prompt;
        private readonly ObservableCollection<String> _items;
        private readonly JavaScriptEngine _engine;
        private IDocument _document;
        private Boolean _readOnly;

        public ReplViewModel()
        {
            _readOnly = false;
            _engine = new JavaScriptEngine();
            _prompt = "$ ";
            _items = new ObservableCollection<String>();
            ClearCommand = new RelayCommand(() => Clear());
            ExecuteCommand = new RelayCommand(cmd => Run(cmd.ToString()));
        }

        public Boolean IsReadOnly
        {
            get { return _readOnly; }
            private set
            {
                _readOnly = value;
                RaisePropertyChanged();
            }
        }

        public IDocument Document
        {
            get { return _document; }
            set { _document = value; Clear(); }
        }

        public ObservableCollection<String> Items
        {
            get { return _items; }
        }

        public String Prompt
        {
            get { return _prompt; }
        }

        public ICommand ClearCommand
        {
            get;
            private set;
        }

        public ICommand ResetCommand
        {
            get;
            private set;
        }

        public ICommand ExecuteCommand
        {
            get;
            private set;
        }

        void Clear()
        {
            _items.Clear();
        }

        void Run(String command)
        {
            _items.Add(_prompt + command);

            try
            {
                var engine = this._engine.GetOrCreateJint(_document);
                var lines = engine.Execute(command).GetCompletionValue().ToString();

                foreach (var line in lines.Split(new[] { "\n" }, StringSplitOptions.None))
                {
                    _items.Add(line);
                }
            }
            catch (JavaScriptException ex)
            {
                _items.Add(ex.Error.ToString());
            }
        }
    }
}