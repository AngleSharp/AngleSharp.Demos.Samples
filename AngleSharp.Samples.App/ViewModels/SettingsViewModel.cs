namespace Samples.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;

    public class SettingsViewModel : BaseViewModel
    {
        ObservableCollection<String> urls;

        public SettingsViewModel()
        {
            urls = new ObservableCollection<String>();

            if (Properties.Settings.Default.TypedUrls == null)
            {
                Properties.Settings.Default.TypedUrls = new StringCollection();
                Properties.Settings.Default.Save();
            }

            foreach (var url in Properties.Settings.Default.TypedUrls)
                urls.Add(url);
        }

        public ObservableCollection<String> Urls
        {
            get { return urls; }
        }

        public void AddUrl(String url)
        {
            if (!Properties.Settings.Default.TypedUrls.Contains(url))
            {
                Properties.Settings.Default.TypedUrls.Add(url);
                Properties.Settings.Default.Save();
                urls.Add(url);
            }
        }
    }
}
