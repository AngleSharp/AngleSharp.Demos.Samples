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
        private readonly String prompt;
        private readonly ObservableCollection<String> items;
        private readonly JavaScriptEngine engine;
        private IDocument document;
        private Boolean readOnly;

        public ReplViewModel()
        {
            readOnly = false;
            engine = new JavaScriptEngine();
            prompt = "$ ";
            items = new ObservableCollection<String>();
            ClearCommand = new RelayCommand(() => Clear());
            ExecuteCommand = new RelayCommand(cmd => Run(cmd.ToString()));
        }

        public Boolean IsReadOnly
        {
            get { return readOnly; }
            private set
            {
                readOnly = value;
                RaisePropertyChanged();
            }
        }

        public IDocument Document
        {
            get { return document; }
            set { document = value; Clear(); }
        }

        public ObservableCollection<String> Items
        {
            get { return items; }
        }

        public String Prompt
        {
            get { return prompt; }
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
            items.Clear();
        }

        void Run(String command)
        {
            items.Add(prompt + command);

            try
            {
                var engine = this.engine.GetOrCreateJint(document);
                var lines = engine.Execute(command).GetCompletionValue().ToString();

                foreach (var line in lines.Split(new[] { "\n" }, StringSplitOptions.None))
                {
                    items.Add(line);
                }
            }
            catch (JavaScriptException ex)
            {
                items.Add(ex.Error.ToString());
            }
        }
    }
}