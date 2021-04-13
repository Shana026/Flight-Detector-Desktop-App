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
        private bool isCsvValid = true;

        public MainPage(string validFlightPath, string flightToDetectPath, string dllPath, AnomalyDetectorType detectorType)
        {
            InitializeComponent();
            try
            {
                MainViewModel mainViewModel = new MainViewModel(validFlightPath);
                DataContext = mainViewModel;
            }
            catch (FormatException e)
            {
                MessageBox.Show("CSV file is not in the right format. Please choose again");
                // ReturnToHomePage();
                isCsvValid = false;
            }
        }

        private void ReturnHomeButton_onClick(object sender, RoutedEventArgs e)
        {
            ReturnToHomePage();
        }

        private void ReturnToHomePage()
        {
            this.NavigationService.Navigate(new HomePage());
        }

        private void ReturnHomeIfCsvNotValid(object sender, RoutedEventArgs e)
        {
            if (!isCsvValid)
                ReturnToHomePage();
        }
    }
}
