namespace Samples.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    public class SettingsViewModel : BaseViewModel
    {
        private readonly ObservableCollection<String> _urls;

        public SettingsViewModel()
        {
            _urls = new ObservableCollection<String>();

            if (Properties.Settings.Default.TypedUrls == null)
            {
                Properties.Settings.Default.TypedUrls = new StringCollection();
                Properties.Settings.Default.Save();
            }

            foreach (var url in Properties.Settings.Default.TypedUrls)
            {
                _urls.Add(url);
            }
        }

        public ObservableCollection<String> Urls
        {
            get { return _urls; }
        }

        public void AddUrl(String url)
        {
            if (!Properties.Settings.Default.TypedUrls.Contains(url))
            {
                Properties.Settings.Default.TypedUrls.Add(url);
                Properties.Settings.Default.Save();
                _urls.Add(url);
            }
        }
    }
}
