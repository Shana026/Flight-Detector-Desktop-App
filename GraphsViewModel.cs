using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FlightDetector.Annotations;
using OxyPlot;

namespace FlightDetector
{
    class GraphsViewModel : INotifyPropertyChanged
    {
        private const double MIN_EPSILON = 0.0000001;
        private const double MAX_EPSILON = 0.9999999;

        public event PropertyChangedEventHandler PropertyChanged;
        private GraphsModel _model;
        private double _secondsPassed = 0;

        private double _timeStepsPerSecond;

        public double TimeStepsPerSecond
        {
            get => this._timeStepsPerSecond;
            set => this._timeStepsPerSecond = value;
        }

        private string[] _features;

        public string[] Features
        {
            get => this._features;
            set => this._features = value;
        }

        private int _timeStep;

        public int TimeStep
        {
            get => this._timeStep;
            set
            {
                this._timeStep = value;
                Trace.WriteLine("in graph model: " + _timeStep); // todo remove
                // we add (1/timeStepsPerSecond) because we want to add the relative part of the second
                this._secondsPassed += (1 / this._timeStepsPerSecond);
                if (IsSecondPassed(this._secondsPassed))
                {

                    this.SelectedLastValues = GetLastValues(this._selectedFeature);
                    this.MostCorrelatedLastValues = GetLastValues(this._mostCorrelatedFeature);
                }
            }
        }

        private string _selectedFeature;

        public string SelectedFeature
        {
            get => this._selectedFeature;
            set
            {
                this._selectedFeature = value;
                this.SelectedLastValues = GetLastValues(this._selectedFeature);
                this.MostCorrelatedFeature = GetMostCorrelatedFeature();
                OnPropertyChanged(nameof(SelectedFeature));
            }
        }

        private List<double> _selectedLastValues;

        public List<double> SelectedLastValues
        {
            get => this._selectedLastValues;
            private set
            {
                this._selectedLastValues = value;
                OnPropertyChanged(nameof(SelectedLastValues));
            }
        }

        public IList<DataPoint> SelectedFeaturePoints { get; private set; }

        private string _mostCorrelatedFeature;

        public string MostCorrelatedFeature
        {
            get => this._mostCorrelatedFeature;
            private set
            {
                this._mostCorrelatedFeature = value;
                this.MostCorrelatedLastValues = GetLastValues(this._mostCorrelatedFeature);
                OnPropertyChanged(nameof(MostCorrelatedFeature));
            }
        }

        private List<double> _mostCorrelatedLastValues;

        public List<double> MostCorrelatedLastValues
        {
            get => this._mostCorrelatedLastValues;
            private set
            {
                this._mostCorrelatedLastValues = value;
                OnPropertyChanged(nameof(MostCorrelatedLastValues));
            }
        }


        public void OnPropertyChanged(string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor 
        public GraphsViewModel(GraphsModel model, double timeStepsPerSecond)
        {
            this._model = model;
            this._timeStepsPerSecond = timeStepsPerSecond;
            this.TimeStep = 0; 
            this.Features = GetFeatures();
            if (this.Features != null)
            {
                this.SelectedFeature = this.Features[0];
            }
            else
            {
                this.SelectedFeature = "";
            }
        }


        // Public ViewModel Methods 

        public string[] GetFeatures()
        {
            return this._model.Data.Features;
        }

        // Private ViewModel Methods

        private bool IsSecondPassed(double time)
        {
            double difference = Math.Abs(Math.Truncate(time) - time);
            return (difference < MIN_EPSILON) || (difference > MAX_EPSILON);
        }

        private List<double> GetLastValues(string feature)
        {
            return this._model.GetLastValues(this._timeStep, this._timeStepsPerSecond, feature);
        }

        private string GetMostCorrelatedFeature()
        {
            return this._model.GetMostCorrelatedFeature(this._selectedFeature);
        }
    }
}
