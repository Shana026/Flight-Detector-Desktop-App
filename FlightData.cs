using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    class FlightData
    {
        private readonly Dictionary<int, float[]> _data;
        
        public int Size { get; }

        public string[] Features { get; }

        FlightData(XmlParser xmlParser, string xmlPath, CsvParser csvParser, string csvPath)
        {
            xmlParser.UploadXml(xmlPath);
            this.Features = xmlParser.GetFeatures();
            this.Size = this.Features.Length;
            this._data = csvParser.DataToDictionary(csvParser.GetCsvLines(csvPath));
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
