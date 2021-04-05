using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    class LastValuesGraphModel
    {
        private FlightData _data;

        public FlightData Data => this._data;


        public List<double> GetLastValues(int timeStep, string feature)
        {
            List<double> lastValues = new List<double>();
            int limit = 30; // todo move to constants
            // we want the last 30 seconds. if the time step is less than 30 then get all that can be fetched
            for (int i = timeStep; i >= 0 || i > timeStep - limit; i--)
            {
                lastValues.Add(this._data.GetFeatureValue(i, feature));
            }

            lastValues.Reverse(); // we got the data in backwards order for convenience. but we want it in the right order
            return lastValues;  
        }
    }
}
