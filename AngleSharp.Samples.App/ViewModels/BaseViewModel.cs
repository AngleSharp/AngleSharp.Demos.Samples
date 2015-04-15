namespace Samples.ViewModels
{
    using AngleSharp;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;

    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected static readonly Char[] ws = new Char[] { ' ', '\n', '\t' };

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] String name = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        protected Url CreateUrlFrom(String address)
        {
            if (File.Exists(address))
                address = "file://localhost/" + address.Replace('\\', '/');

            var lurl = address.ToLower();

            if (!lurl.StartsWith("file://") && !lurl.StartsWith("http://") && !lurl.StartsWith("https://"))
                address = "http://" + address;

            var url = Url.Create(address);

            if (url.IsInvalid == false && url.IsAbsolute)
                return url;

            return Url.Create("http://www.google.com/search?q=" + address);
        }
    }
}
