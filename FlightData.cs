using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    class FlightData
    {
        private Dictionary<int, float[]> _data;

        private string[] _features;

        public string[] Features => this._features;

        FlightData(string xmlPath, string csvPath)
        {
            // todo implement
            _data = new Dictionary<float, float[]>();
            _features = new string[10]; // todo size of array
        }

        public float[] GetTimeStepData(int timeStep)
        {
            return this._data[timeStep];
        }

        public float GetFeatureValue(int timeStep, string feature)
        {
            // todo implement
            return 1;
        }
    }
}
