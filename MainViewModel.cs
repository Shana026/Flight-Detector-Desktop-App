﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    class MainViewModel
    {
        private const string XML_PATH = "C:\\Users\\Shana\\source\\repos\\FlightDetector\\playback_small.xml";
        private const int DEFAULT_SPEED = 100;

        private int _timeStep;

        public int TimeStep
        {
            get { return _timeStep; }
            set
            {
                _timeStep = value;
                Trace.WriteLine("in main viewModel: " + _timeStep); // todo remove
                UpdateViewModelsTimeStepChanged();
            }
        }

        private int _speed;

        public int Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                UpdateViewModlesSpeedChanged();
            }
        }


        private FooterViewModel _footerViewModel;

        public FooterViewModel FooterViewModel
        {
            get { return _footerViewModel; }
            set
            { _footerViewModel = value; }
        }


        private joystickViewModel _joystickViewModel;

        public joystickViewModel JoystickViewModel
        {
            get { return _joystickViewModel; }
            set { _joystickViewModel = value; }
        }


        private dataDisplayViewModel _dataDisplayViewModel;

        public dataDisplayViewModel DataDisplayViewModel
        {
            get { return _dataDisplayViewModel; }
            set { _dataDisplayViewModel = value; }
        }


        private GraphsViewModel _graphsViewModel;

        public GraphsViewModel GraphsViewModel
        {
            get { return _graphsViewModel; }
            set { _graphsViewModel = value; }
        }


        // Constructor

        public MainViewModel(string csvPath) // todo how to get path?
        {
            AnomalyDetector anomalyDetector = new AnomalyDetector("", "",0); //added
            anomalyDetector.learnNormalFromCSV(new StringBuilder("train.csv")); //added
            anomalyDetector.detectFromCSV(new StringBuilder("test.csv")); //added
        
            XmlParser xmlParser = new XmlParser();
            string xmlPath = XML_PATH;
            CsvParser csvParser = new CsvParser();
            FlightData data = new FlightData(xmlParser, xmlPath, csvParser, csvPath);

            this.FooterViewModel = new FooterViewModel(new MyFooterModel(new Client(),anomalyDetector));  //added anomalyDetector
            this.FooterViewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs args)
            {
                OnFooterPropertyChange(args);
            };

            this.JoystickViewModel = new joystickViewModel(new joystickModel(data));

            this.DataDisplayViewModel = new dataDisplayViewModel(new dataDisplayModel(data));

            this.GraphsViewModel = new GraphsViewModel(new GraphsModel(data), (int)1000 / DEFAULT_SPEED);
        }

        private void OnFooterPropertyChange(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "VM_NextLine")
            {
                this.TimeStep = this.FooterViewModel.VM_NextLine;
            }

            if (args.PropertyName == "VM_PlaybackSpeed")
            {
                this.Speed = this.FooterViewModel.VM_PlaybackSpeed;
            }
        }

        private void UpdateViewModelsTimeStepChanged()
        {
            this.GraphsViewModel.TimeStep = this.TimeStep;
            this.JoystickViewModel.TimeStep = this.TimeStep;
            this.DataDisplayViewModel.TimeStep = this.TimeStep;
            // todo add other viewModels here
        }

        private void UpdateViewModlesSpeedChanged()
        {
            this.GraphsViewModel.TimeStepsPerSecond = (int)1000 / this.Speed;
        }
    }
}
