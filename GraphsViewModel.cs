using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FlightDetector.Annotations;
//using OxyPlot;
// using OxyPlot.Series;
using LiveCharts;
using LiveCharts.Wpf;

namespace FlightDetector
{
    class GraphsViewModel : INotifyPropertyChanged
    {
        private const double MIN_EPSILON = 0.0000001;
        private const double MAX_EPSILON = 0.9999999;

        public event PropertyChangedEventHandler PropertyChanged;
        private GraphsModel _model;
        private double _secondsPassed = 0;
        private int _lastTimeStepAdded = 0;

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
                // bool isBackwards = IsBackwards(value, this._timeStep);
                this._timeStep = value;
                Trace.WriteLine("in graph model: " + _timeStep); // todo remove
                // we add (1/timeStepsPerSecond) because we want to add the relative part of the second
                this._secondsPassed += (1 / this._timeStepsPerSecond);
                if (IsSecondPassed(this._secondsPassed))
                {
                    UpdateLastValues(this.SelectedFeatureChartValues, SelectedFeature);
                    UpdateLastValues(this.MostCorrelatedChartValues, MostCorrelatedFeature);
                    this._lastTimeStepAdded = this._timeStep;
                    // this.SelectedLastValues = GetLastValues(this._selectedFeature);
                    // this.MostCorrelatedLastValues = GetLastValues(this._mostCorrelatedFeature);
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
                // this.SelectedLastValues = GetLastValues(this._selectedFeature);
                this._lastTimeStepAdded = 0;
                this.SelectedFeatureChartValues.Clear();
                UpdateLastValues(this.SelectedFeatureChartValues, this._selectedFeature);
                this.MostCorrelatedFeature = GetMostCorrelatedFeature();
                OnPropertyChanged(nameof(SelectedFeature));
            }
        }

        // private List<double> _selectedLastValues;

        /*
        public List<double> SelectedLastValues
        {
            get => this._selectedLastValues;
            private set
            {
                this._selectedLastValues = value;
                UpdateChartValues(this.SelectedFeatureChartValues, _selectedLastValues);
                OnPropertyChanged(nameof(SelectedLastValues));
            }
        }
        */

        public SeriesCollection SelectedFeatureGraph { get; set; }

        public ChartValues<double> SelectedFeatureChartValues { get; set; }

        private string _mostCorrelatedFeature;

        public string MostCorrelatedFeature
        {
            get => this._mostCorrelatedFeature;
            set
            {
                this._mostCorrelatedFeature = value;
                // this.MostCorrelatedLastValues = GetLastValues(this._mostCorrelatedFeature);
                this.MostCorrelatedChartValues.Clear();
                UpdateLastValues(this.MostCorrelatedChartValues, this._mostCorrelatedFeature);
                OnPropertyChanged(nameof(MostCorrelatedFeature));
            }
        }

        /*
        private List<double> _mostCorrelatedLastValues;

        public List<double> MostCorrelatedLastValues
        {
            get => this._mostCorrelatedLastValues;
            private set
            {
                this._mostCorrelatedLastValues = value;
                UpdateChartValues(this.MostCorrelatedChartValues, _mostCorrelatedLastValues);
                OnPropertyChanged(nameof(MostCorrelatedLastValues));
            }
        }
        */

        public SeriesCollection MostCorrelatedGraph { get; set; }

        public ChartValues<double> MostCorrelatedChartValues { get; set; }

        // public Func<double, string> YFormmater { get; set; }

        public void OnPropertyChanged(string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor 
        public GraphsViewModel(GraphsModel model, double timeStepsPerSecond)
        {
            this.SelectedFeatureChartValues = new ChartValues<double>();
            this.MostCorrelatedChartValues = new ChartValues<double>();
            this._model = model;
            this._timeStepsPerSecond = timeStepsPerSecond;
            this.TimeStep = 0; 
            this.Features = GetFeatures();
            this.SelectedFeature = this.Features != null ? this.Features[0] : "";

            UpdateLastValues(this.SelectedFeatureChartValues, this.SelectedFeature);
            UpdateLastValues(this.MostCorrelatedChartValues, this.SelectedFeature);
            // UpdateChartValues(this.SelectedFeatureChartValues, SelectedLastValues);
            // UpdateChartValues(this.MostCorrelatedChartValues, MostCorrelatedLastValues);

            this.SelectedFeatureGraph = new SeriesCollection
            {
                new LineSeries
                {
                    // Title = SelectedFeature,
                    Values = this.SelectedFeatureChartValues,
                    //  LabelPoint = point => String.Format("{0:0.00}", point),
                }
            };

            this.MostCorrelatedGraph = new SeriesCollection
            {
                new LineSeries
                {
                    // Title = MostCorrelatedFeature,
                    Values = this.MostCorrelatedChartValues
                }
            };

            // YFormmater = value => value.ToString("C") + " hi";
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

        private bool IsBackwards(int currentTimeStep, int lastTimeStep)
        {
            return lastTimeStep > currentTimeStep;
        }

        private void UpdateLastValues(ChartValues<double> values, string feature)
        {
            if (IsBackwards(this._timeStep, this._lastTimeStepAdded))
            {

                for (int i = values.Count - 1; i >= _timeStep; i--)
                {
                    values.RemoveAt(i);
                }
            }
            else
            {
                double[] valuesToAdd = this._model.GetRangeValues(this._lastTimeStepAdded + 1, this._timeStep, feature);
                foreach (double value in valuesToAdd)
                {
                    values.Add(value);
                }
            }
        }

        private List<double> GetLastValues(string feature)
        {
            return this._model.GetLastValues(this._timeStep, this._timeStepsPerSecond, feature);
        }

        private string GetMostCorrelatedFeature()
        {
            return this._model.GetMostCorrelatedFeature(this._selectedFeature);
        }

        private void UpdateChartValues(ChartValues<double> chartValues, List<double> values)
        {
            /*
            SelectedFeatureChartValues.Clear();
            {
                for (int i = 0; i < SelectedLastValues.Count; i++)
                {
                    SelectedFeatureChartValues.Add(SelectedLastValues[i]);
                }
            }
            */
            chartValues.Clear();
            foreach (double value in values)
            {
                chartValues.Add(value);
            }
        }
    }
}
