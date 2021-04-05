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
                    UpdateLastValues();
                }
            }
        }

        private List<double> _lastValues;

        public List<double> LastValues
        {
            get => this._lastValues;
            private set
            {
                this._lastValues = value;
                OnPropertyChanged(nameof(LastValues));
            }
        }
        private string _selectedFeature;

        public string SelectedFeature
        {
            get => this._selectedFeature;
            private set
            {
                this._selectedFeature = value;
                UpdateLastValues();
                OnPropertyChanged(nameof(SelectedFeature));
            }
        }

        public void OnPropertyChanged(string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void UpdateLastValues()
        {
            LastValues = this._model.GetLastValues(this._timeStep, this._selectedFeature);
        }
    }
}
