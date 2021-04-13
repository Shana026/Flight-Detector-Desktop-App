using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    class FlightData
    {
        public int[] anomaliesList = { 1, 50, 100, 200, 500, 645, 234, 900, 1000, 1500 }; //added

        private readonly Dictionary<int, double[]> _data;

        public int Size { get; }

        public string[] Features { get; }

        public FlightData(XmlParser xmlParser, string xmlPath, CsvParser csvParser, string csvPath)
        {
            xmlParser.UploadXml(xmlPath);
            this.Features = xmlParser.GetFeatures();
            this.Size = this.Features.Length;
            this._data = csvParser.DataToDictionary(csvParser.GetCsvLines(csvPath));
        }

        public double[] GetTimeStepData(int timeStep)
        {
            return this._data[timeStep];
        }

        public double GetFeatureValue(int timeStep, string feature)
        {
            int featureIndex = GetFeatureIndex(feature);
            return this._data[timeStep][featureIndex];
        }

        public double[] GetFeatureAllValues(string feature)
        {
            double[] allValues = new double[this.Size];
            for (int i = 0; i < this.Size; i++)
            {
                allValues[i] = GetFeatureValue(i, feature);
            }

            return allValues;
        }

        private int GetFeatureIndex(string feature)
        {
            for (int i = 0; i < this.Size; i++)
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