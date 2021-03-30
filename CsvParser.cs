using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FlightDetector
{
    class CsvParser
    {
        public string[] GetCsvLines(string path)
        {
            return File.ReadAllLines(path);
        }

        public Dictionary<int, float[]> DataToDictionary(string[] lines)
        {
            Dictionary<int, float[]> data = new Dictionary<int, float[]>();
            int timeStep = 0;
            foreach (string line in lines)
            {
                float[] values = StringArrayToFloat(line.Split(','));
                data.Add(timeStep, values);

                timeStep++;
            }

            return data;
        }

        private float[] StringArrayToFloat(string[] stringArr)
        {
            float[] floatArr = new float[stringArr.Length];
            try
            {
                for (int i = 0; i < floatArr.Length; i++)
                {
                    floatArr[i] = float.Parse(stringArr[i]);
                }
            }
            catch (Exception e)
            {
                throw new Exception("CSV values cannot convert to floats");
            }

            return floatArr;
        }
    }
}
