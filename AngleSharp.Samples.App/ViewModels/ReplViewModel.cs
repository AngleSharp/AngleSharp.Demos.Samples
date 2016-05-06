namespace Samples.ViewModels
{
    using AngleSharp.Dom;
    using AngleSharp.Network;
    using AngleSharp.Scripting.JavaScript;
    using AngleSharp.Services.Scripting;
    using Jint.Runtime;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading;
    using System.Windows.Input;

    sealed class ReplViewModel : BaseViewModel, ITabViewModel
    {
        readonly String prompt;
        readonly ObservableCollection<String> items;
        readonly JavaScriptEngine engine;
        IDocument document;
        Boolean readOnly;

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
                var options = new ScriptOptions(document);
                var response = VirtualResponse.Create(res => res.Content(command));
                engine.EvaluateScriptAsync(response, options, CancellationToken.None).Wait();
                var lines = engine.GetJint(document).GetCompletionValue().ToString();

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