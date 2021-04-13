using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightDetector
{
    /// <summary>
    /// Interaction logic for LastValuesGraphView.xaml
    /// </summary>
    public partial class GraphsView : UserControl
    {
        public GraphsView()
        {
            InitializeComponent();
        }


        private void ListenToViewModel(object sender, RoutedEventArgs e)
        {
            ((GraphsViewModel)this.DataContext).PropertyChanged += (o, args) =>
            {
                if (args.PropertyName == "MostCorrelatedFeature" && string.IsNullOrEmpty(((GraphsViewModel) DataContext).MostCorrelatedFeature))
                {
                    this.MostCorrelatedGraph.Visibility = Visibility.Hidden;
                    this.MostCorrelatedTextBlock.Visibility = Visibility.Hidden;
                    this.NoMostCorrelatedTextBlock.Visibility = Visibility.Visible;
                }
                else
                {
                    Trace.WriteLine("Some property changed");
                }
            };
        }
    }
}
