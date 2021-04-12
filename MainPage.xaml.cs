using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using FlightDetector.Annotations;

namespace FlightDetector
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage(string validFlightPath, string flightToDetectPath, string dllPath, AnomalyDetectorType detectorType)
        {
            InitializeComponent();
            MainViewModel mainViewModel = new MainViewModel(validFlightPath);
            DataContext = mainViewModel;
        }
    }
}
