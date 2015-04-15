namespace Samples.Pages
{
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for Profiler.xaml
    /// </summary>
    public partial class Profiler : UserControl
    {
        public Profiler()
        {
            InitializeComponent();
            //vm.PropertyChanged += (s, e) => plot.RefreshPlot(true);
        }
    }
}
