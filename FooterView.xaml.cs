using FlightDetector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Footer.xaml
    /// </summary>
    public partial class FooterView : UserControl
    {
        FooterViewModel vm;
        public FooterView()
        {
            InitializeComponent();
            vm = new FooterViewModel(new MyFooterModel(new Client()));
            DataContext = vm;
            vm.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                Trace.WriteLine("in view next line ValueChanged\n");
            };
            ComboBox speeds = new ComboBox();
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_play = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Trace.WriteLine("in ComboBox_SelectionChanged");
            Handle();

        }
        private void Handle()
        {
           
            switch (speeds.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "0.25":
                    vm.VM_playbackSpeed = 400;
                    Trace.WriteLine("0.25");
                    //Handle for the first combobox
                    break;
                case "0.5":
                    vm.VM_playbackSpeed = 200;
                    Trace.WriteLine("0.5");
                    //Handle for the second combobox
                    break;
                case "0.75":
                    vm.VM_playbackSpeed = 133;
                    Trace.WriteLine("0.75");
                    //Handle for the third combobox
                    break;
                case "Normal":
                    vm.VM_playbackSpeed = 100;
                    Trace.WriteLine("Normal");
                    //Handle for the first combobox
                    break;
                case "1.25":
                    vm.VM_playbackSpeed = 80;
                    Trace.WriteLine("1.25");
                    //Handle for the second combobox
                    break;
                case "1.5":
                    vm.VM_playbackSpeed = 66;
                    Trace.WriteLine("1.5");
                    //Handle for the third combobox
                    break;
                case "1.75":
                    vm.VM_playbackSpeed = 57;
                    Trace.WriteLine("1.75");
                    //Handle for the second combobox
                    break;
                case "2":
                    vm.VM_playbackSpeed = 50;
                    Trace.WriteLine("2");
                    //Handle for the third combobox
                    break;
            }
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Trace.WriteLine("in view slider_ValueChanged\n");
            Trace.WriteLine($"slider.Value.ToString(): {slider.Value.ToString()}\n");
            vm.VM_NextLine = Convert.ToInt32(slider.Value.ToString());
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            vm.stopPlay();
        }
    }
}
