using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using FlightDetector.Annotations;

namespace FlightDetector
{
    class LastValuesGraphViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private LastValuesGraphModel _model;
        private double _secondsPassed = 0;
        private double _timeStepsPerSecond;

        private int _timeStep;

        public int TimeStep
        {
            get => this._timeStep;
            set
            {
                this._timeStep = value;
                // we add (1/timeStepsPerSecond) because we want to add the relative part of the second
                if (IsSecondPassed(this._secondsPassed + (1 / this._timeStepsPerSecond)))
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
            private set
            {
                this._selectedFeature = value;
                this.SelectedLastValues = GetLastValues(this._selectedFeature);
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
        public LastValuesGraphViewModel(LastValuesGraphModel model, double timeStepsPerSecond, int timeStep)
        {
            this._model = model;
            // todo this._model.PropertyChanged += delegate(Object sender, PropertyChangedEventsArgs e) {NotifyPropertyChanged(e.PropertyName);};
            this._timeStepsPerSecond = timeStepsPerSecond;
            this._timeStep = timeStep; // Todo remove?
        }


        // Public ViewModel Methods 

        public string[] GetFeatures()
        {
            return this._model.Data.Features;
        }

        // Private ViewModel Methods

        private bool IsSecondPassed(double time)
        {
            return (time % 1) == 0;
        }

        private List<double> GetLastValues(string feature)
        {
            return this._model.GetLastValues(this._timeStep, feature);
        }

        private string GetMostCorrelatedFeature()
        {
            return this._model.GetMostCorrelatedFeature(this._selectedFeature);
        }
    }
}
