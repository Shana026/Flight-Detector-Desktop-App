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
        private int _size;

        public string[] Features { get; }

        FlightData(string xmlPath, string csvPath)
        {
            // todo implement
            _data = new Dictionary<float, float[]>();
            Features = new string[10]; // todo size of array
        }

        public float[] GetTimeStepData(int timeStep)
        {
            return this._data[timeStep];
        }

        public float GetFeatureValue(int timeStep, string feature)
        {
            int featureIndex = GetFeatureIndex(feature);
            return this._data[timeStep][featureIndex];
        }

        private int GetFeatureIndex(string feature)
        {
            for (int i = 0; i < this._size; i++)
            {
                if (feature == Features[i])
                {
                    return i;
                }
            }

            throw new Exception("Feature does not exist");
        }
    }
}
