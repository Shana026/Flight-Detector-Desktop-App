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
                // this.SelectedFeaturePoints = BuildPoints(this._selectedLastValues);
                // this.SeriesCollection = BuildSeriesCollection(this.SelectedLastValues);
                UpdateChartValues();
                OnPropertyChanged(nameof(SelectedLastValues));
            }
        }

        public SeriesCollection SeriesCollection { get; set; }

        public ChartValues<double> ChartValues { get; set; }

        /*
        private IList<DataPoint> _selectedFeaturePoints;

        public IList<DataPoint> SelectedFeaturePoints
        {
            get => this._selectedFeaturePoints;
            private set
            {
                this._selectedFeaturePoints = value;
                OnPropertyChanged(nameof(SelectedFeaturePoints));
            }
        }
        */

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
                // this.MostCorrelatedPoints = BuildPoints(this._selectedLastValues);
                OnPropertyChanged(nameof(MostCorrelatedLastValues));
            }
        }

        /*
        private IList<DataPoint> _mostCorrelatedPoints;

        public IList<DataPoint> MostCorrelatedPoints
        {
            get => this._mostCorrelatedPoints;
            private set
            {
                this._mostCorrelatedPoints = value;
                OnPropertyChanged(nameof(MostCorrelatedPoints));
            }
        }

        */

        public void OnPropertyChanged(string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor 
        public GraphsViewModel(GraphsModel model, double timeStepsPerSecond)
        {
            this.ChartValues = new ChartValues<double>();
            this._model = model;
            this._timeStepsPerSecond = timeStepsPerSecond;
            this.TimeStep = 0; 
            this.Features = GetFeatures();
            // this.SelectedFeature = this.Features != null ? this.Features[0] : "";
            this.SelectedFeature = "roll-deg"; // todo remove
            UpdateChartValues();

            this.SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Values = this.ChartValues
                }
            };
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

        /*
        private IList<DataPoint> BuildPoints(List<double> values)
        {
            IList<DataPoint> points = new List<DataPoint>();
            for (int i = 0; i < values.Count; i++)
            {
                DataPoint point = new DataPoint(i, values[i]);
                points.Add(point);
            }

            return points;
        }
        */

        private SeriesCollection BuildSeriesCollection(List<double> values)
        {
            ChartValues<double> cahrtValues = new ChartValues<double>();
            for (int i = 0; i < values.Count; i++)
            {
                cahrtValues.Add(values[i]);
            }

            SeriesCollection series = new SeriesCollection();
            Application.Current.Dispatcher.Invoke((Action)delegate {
                // your code
                series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = cahrtValues
                    }
                };
            });
            
            return series;
        }

        private void UpdateChartValues()
        {
            ChartValues.Clear();
            {
                for (int i = 0; i < SelectedLastValues.Count; i++)
                {
                    ChartValues.Add(SelectedLastValues[i]);
                }
            }
        }
    }
}
