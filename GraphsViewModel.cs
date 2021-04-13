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
using LiveCharts;
using LiveCharts.Defaults;
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
                this._timeStep = value;
                Trace.WriteLine("in graph model: " + _timeStep); // todo remove
                // we add (1/timeStepsPerSecond) because we want to add the relative part of the second
                this._secondsPassed += (1 / this._timeStepsPerSecond);
                if (IsSecondPassed(this._secondsPassed))
                {
                    UpdateLastValues(this.SelectedFeatureChartValues, SelectedFeature);
                    UpdateLastValues(this.MostCorrelatedChartValues, MostCorrelatedFeature);
                    this._lastTimeStepAdded = this._timeStep;
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
                this._lastTimeStepAdded = 0;
                this.SelectedFeatureChartValues.Clear();
                UpdateLastValues(this.SelectedFeatureChartValues, this._selectedFeature);
                this.MostCorrelatedFeature = GetMostCorrelatedFeature();
                OnPropertyChanged(nameof(SelectedFeature));
            }
        }

        public SeriesCollection SelectedFeatureGraph { get; set; }

        public ChartValues<double> SelectedFeatureChartValues { get; set; }

        private string _mostCorrelatedFeature;

        public string MostCorrelatedFeature
        {
            get => this._mostCorrelatedFeature;
            set
            {
                this._mostCorrelatedFeature = value;
                if (this._mostCorrelatedFeature != null)
                {
                    this.MostCorrelatedChartValues.Clear();
                    UpdateLastValues(this.MostCorrelatedChartValues, this._mostCorrelatedFeature);
                }
                OnPropertyChanged(nameof(MostCorrelatedFeature));
            }
        }

        public SeriesCollection MostCorrelatedGraph { get; set; }

        public ChartValues<double> MostCorrelatedChartValues { get; set; }

        public void OnPropertyChanged(string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        // todo add max values of axes

        public SeriesCollection AnomalyDetectionGraph { get; set; }
        public ChartValues<ScatterPoint> ThresholdValues { get; set; }
        public ChartValues<ScatterPoint> NormalFeaturesValues { get; set; }
        public ChartValues<ScatterPoint> AnomalyFeaturesValues { get; set; }


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

            this.SelectedFeatureGraph = new SeriesCollection
            {
                new LineSeries
                {
                    Values = this.SelectedFeatureChartValues
                }
            };

            this.MostCorrelatedGraph = new SeriesCollection
            {
                new LineSeries
                {
                    Values = this.MostCorrelatedChartValues
                }
            };

            // this.NormalFeaturesValues = new ChartValues<ScatterPoint>();
            // this.AnomalyFeaturesValues = new ChartValues<ScatterPoint>();
            // this.ThresholdValues = new ChartValues<ScatterPoint>();
            // this.AnomalyDetectionGraph = BuildDetectionSeriesCollection();
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

        private string GetMostCorrelatedFeature()
        {
            return this._model.GetMostCorrelatedFeature(this._selectedFeature);
        }

        private SeriesCollection BuildDetectionSeriesCollection()
        {
            this.ThresholdValues.Clear(); // when building the graph we need to erase what was there before

            BuildFeatureValues(this.NormalFeaturesValues, AnomalyFeaturesValues);
            if (this._model.GetDetectorType() == AnomalyDetectorType.LinearRegression)
            {
                this.ThresholdValues = BuildLinearRegressionValues();
            }
            else // Min Circle
            {
                this.ThresholdValues = BuildMinCircleValues();
            }

            SeriesCollection detectionSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Values = this.ThresholdValues
                },
                new ScatterSeries
                {
                    Values = this.NormalFeaturesValues
                },
                new ScatterSeries
                {
                    Values = this.AnomalyFeaturesValues
                }
            };

            return detectionSeries;
        }

        private void BuildFeatureValues(ChartValues<ScatterPoint> normal, ChartValues<ScatterPoint> anomaly)
        {
            List<double> selectedFeatureValues =
                this._model.GetLastValues(this.TimeStep, this.TimeStepsPerSecond, this.SelectedFeature);
            List<double> mostCorrelatedValues =
                this._model.GetLastValues(this.TimeStep, this.TimeStepsPerSecond, this.MostCorrelatedFeature);

            normal.Clear();
            anomaly.Clear();

            int index = 0;
            for (int i = this.TimeStep - 30; i < selectedFeatureValues.Count; i++) // todo to constant
            {
                if (IsTimeStepAnomaly(i))
                {
                    anomaly.Add(new ScatterPoint(selectedFeatureValues[index], mostCorrelatedValues[index]));
                }
                else
                {
                    normal.Add(new ScatterPoint(selectedFeatureValues[index], mostCorrelatedValues[index]));
                }
            }
        }

        private bool IsTimeStepAnomaly(int timeStep)
        {
            int[] allAnomaliesTimeSteps = this._model.GetAllAnomaliesTimeSteps();
            return Array.Exists(allAnomaliesTimeSteps, anomaly => timeStep == anomaly);
        }

        private ChartValues<ScatterPoint> BuildLinearRegressionValues()
        {
            var correlationData = this._model.GetCorrelationData();
            float[] line = correlationData[this.SelectedFeature].Value;

            double beginX = GetMinXValue() - GetMinXValue() * 0.1;
            double beginY = line[0] * beginX + line[1];

            double endX = GetMaxXValue() + GetMaxXValue() * 0.1;
            double endY = line[0] * endX + line[1];

            ChartValues<ScatterPoint> lineChartValues = new ChartValues<ScatterPoint>
            {
                new(beginX, beginY),
                new(endX, endY)
            };

            return lineChartValues;
        }

        private double GetMaxXValue()
        {
            // todo max between normal and anomaly
            double max = this.NormalFeaturesValues[0].X;
            foreach (ScatterPoint point in this.NormalFeaturesValues)
            {
                if (max < point.X)
                {
                    max = point.X;
                }
            }

            return max;
        }

        private double GetMinXValue()
        {
            // todo min between normal and anomaly
            double min = this.NormalFeaturesValues[0].X;
            foreach (ScatterPoint point in this.NormalFeaturesValues)
            {
                if (min > point.X)
                {
                    min = point.X;
                }
            }

            return min;
        }

        private ChartValues<ScatterPoint> BuildMinCircleValues()
        {
            var correlationData = this._model.GetCorrelationData();

            float[] circleCenterAndRadius = correlationData[this.SelectedFeature].Value;

            double centerX = circleCenterAndRadius[0];
            double radius = circleCenterAndRadius[2];

            ChartValues<ScatterPoint> circle = new ChartValues<ScatterPoint>();

            double i = 0;
            while (i < centerX + radius)
            {
                double[] res = GetCircleY(circleCenterAndRadius, i);
                ScatterPoint p1 = new ScatterPoint(i, res[0], 1);
                ScatterPoint p2 = new ScatterPoint(i, res[1], 1);
                circle.Add(p1);
                circle.Add(p2);

                if (i - centerX + radius < 0.05 || centerX + radius - i < 0.05)
                {
                    i += 0.001;
                }
                else
                {
                    i += 0.01;
                }
            }

            return circle;
        }

        private double[] GetCircleY(float[] circle, double x)
        {
            // according to circle equation
            double centerX = circle[0];
            double centerY = circle[1];
            double radius = circle[2];

            double xElement = Math.Pow((centerX - x), 2);
            double radiusSquared = Math.Pow(radius, 2);
            double centerYSquared = Math.Pow(centerY, 2);

            double inSqrt = 4 * centerYSquared - 4 * (xElement + centerYSquared - radiusSquared);

            double y1 = (2 * centerY + Math.Sqrt(inSqrt)) / 2;
            double y2 = (2 * centerY - Math.Sqrt(inSqrt)) / 2;

            double[] result = {y1, y2};

            return result;
        }
    }
}
