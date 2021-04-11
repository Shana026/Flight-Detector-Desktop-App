using System;
using System.Collections.Generic;
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
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private string validFlightPath;
        private string flightToDetectPath;
        private string dllPath;
        private AnomalyDetectorType detectorType;

        public HomePage()
        {
            InitializeComponent();
            ShowsNavigationUI = false;
            DataContext = this;
        }

        private void UploadValidFlightButton_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV Files (*.csv)|*.csv";

            Nullable<bool> chosenFile = dialog.ShowDialog();

            if (chosenFile == true)
            {
                validFlightPath = dialog.FileName;
            }
        }

        private void UploadDllButton_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".dll";
            dialog.Filter = "DLL Files (*.dll)|*.dll";

            Nullable<bool> chosenFile = dialog.ShowDialog();

            if (chosenFile == true)
            {
                dllPath = dialog.FileName;
            }
        }

        private void UploadFlightToDetectButton_OnClick(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.DefaultExt = ".csv";
            dialog.Filter = "CSV Files (*.csv)|*.csv";

            Nullable<bool> chosenFile = dialog.ShowDialog();

            if (chosenFile == true)
            {
                flightToDetectPath = dialog.FileName;
            }
        }
    }
}
