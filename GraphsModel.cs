using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    class GraphsModel
    {
        private const int SECONDS_LIMIT = 30;

        private FlightData _data;

        public FlightData Data => this._data;


        public GraphsModel(FlightData data)
        {
            this._data = data;
        }


        public List<double> GetLastValues(int timeStep, double timeStepPerSecond, string feature)
        {
            List<double> lastValues = new List<double>();
            int limit = SECONDS_LIMIT * (int)timeStepPerSecond;
            // we want the last 30 seconds. if the time step is less than 30 then get all that can be fetched
            for (int i = timeStep; i >= 0 && i > timeStep - limit; i -= (int)timeStepPerSecond)
            {
                lastValues.Add(this._data.GetFeatureValue(i, feature));
            }

            lastValues.Reverse(); // we got the data in backwards order for convenience. but we want it in the right order
            return lastValues;
        }

        public string GetMostCorrelatedFeature(string feature)
        {
            double maxCorrelation = 0;
            string mostCorrelated = "";
            double[] featureAllValues = this._data.GetFeatureAllValues(feature);
            string[] features = this._data.Features;
            // todo maybe we need to check only features that are of greater column (like in semester A)
            for (int i = 0; i < features.Length; i++)
            {
                if (features[i] == feature) // we don't want to check correlation to the same feature
                {
                    continue;
                }
                double[] tempFeatureAllValues = this._data.GetFeatureAllValues(features[i]);
                double tempCorrelation = Math.Abs(StatisticsUtil.Pearson(featureAllValues, tempFeatureAllValues));

                if (tempCorrelation <= maxCorrelation)
                {
                    continue;
                }

                maxCorrelation = tempCorrelation;
                mostCorrelated = features[i];
            }

            return mostCorrelated;
        }
    }
}