namespace Samples
{
    using MahApps.Metro.Controls;
    using Samples.ViewModels;
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        readonly MainViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new MainViewModel();
            DataContext = vm;
        }

        void UrlChanged(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                vm.Go();
        }

        void GoClicked(Object sender, RoutedEventArgs e)
        {
            vm.Go();
        }
    }
}
