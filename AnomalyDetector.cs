using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    enum AnomalyDetectorType
    {
        LinearRegression, MinCircle
    }

    class AnomalyDetector
    {
        private string _dllPath;
        private string _csvPath;
        private AnomalyDetectorType _detectorType;

        public AnomalyDetectorType DetectorType
        {
            get => this._detectorType;
            set => this._detectorType = value;
        }

        public AnomalyDetector(string dllPath, string csvPath, AnomalyDetectorType detectorType)
        {
            this._dllPath = dllPath;
            this._csvPath = csvPath;
            this.DetectorType = detectorType;
        }

        public string GetMostCorrelativeFeature(string feature)
        {
            return ""; // todo implement
        }

        public float[] GetLinearRegression(string feature)
        {
            return new float[2]; // todo implement
        }

        public int[] GetAllAnomaliesTimesSteps()
        {
            return new int[10]; // todo implement
        }
    }
}
