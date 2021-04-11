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
    public partial class MainPage : Page, INotifyPropertyChanged
    {
        public string _validFlight;
        public string ValidFlight
        {
            get => _validFlight;
            set
            {
                _validFlight = value;
                OnPropertyChanged(nameof(ValidFlight));
            }
        }

        public string _flightToDetect;
        public string FlightToDetect
        {
            get => _flightToDetect;
            set
            {
                _flightToDetect = value;
                OnPropertyChanged(nameof(FlightToDetect));
            }
        }

        public string _dllPath;

        public string DllPath
        {
            get => _dllPath;
            set
            {
                _dllPath = value;
                OnPropertyChanged(nameof(DllPath));
            }
        }

        public string _detectorType;
        public string DetectorType
        {
            get => _detectorType;
            set
            {
                _detectorType = value;
                OnPropertyChanged(nameof(DetectorType));
            }
        }

        public MainPage(string validFlightPath, string flightToDetectPath, string dllPath, AnomalyDetectorType detectorType)
        {
            InitializeComponent();
            ValidFlight = validFlightPath;
            FlightToDetect = flightToDetectPath;
            DllPath = dllPath;
            if (detectorType == AnomalyDetectorType.LinearRegression)
            {
                DetectorType = "Linear Regression";
            }
            else if (detectorType == AnomalyDetectorType.MinCircle)
            {
                DetectorType = "Min Circle";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
