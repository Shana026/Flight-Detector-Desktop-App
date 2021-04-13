using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FlightDetector
{
    public enum AnomalyDetectorType
    {
        LinearRegression, MinCircle
    }

    class AnomalyDetector
    {
        private string _dllPath;
        public string CsvPath { get; private set; }
        private AnomalyDetectorType _detectorType;
        IntPtr anomalyDetector;

        public AnomalyDetectorType DetectorType
        {
            get => this._detectorType;
            set => this._detectorType = value;
        }
        //function from the DLL
        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern IntPtr create();

        //learn normal
        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern int getMostCorrelativeFeature(IntPtr a, StringBuilder feature);

        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern int getLenOfStringWrapper(IntPtr a);

        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern char getCharByIndexStringWrapper(IntPtr a, int x);

        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern int learnNormalFromCSV(IntPtr a, StringBuilder CSVfileName);

        //get Linear  Regression 
        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern void getLinearRegression(IntPtr a, StringBuilder feature);

        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern int getLenOfFloatArrayWrapper(IntPtr a);

        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern float getFloatArrayByIndex(IntPtr a, int index);

        //detect
        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern int detectFromCSV(IntPtr a, StringBuilder CSVfileName);

        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern int getAllAnomalyTimestamp(IntPtr a);

        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern int getLenOfArrayWrapper(IntPtr a);

        [DllImport("SimpleAnomalyDetector.dll")]
        public static extern int getAnomalyByIndex(IntPtr a, int index);

        public AnomalyDetector(string dllPath, string csvPath, AnomalyDetectorType detectorType)
        {
            this._dllPath = dllPath;
            this.CsvPath = csvPath;
            this.DetectorType = detectorType;
            this.anomalyDetector = create();
        }
         public int learnNormalFromCSV(StringBuilder CSVfileName)
        {
            return learnNormalFromCSV(anomalyDetector, CSVfileName);
        }
        public int getLenOfStringBuilder()
        {
            return getLenOfStringWrapper(anomalyDetector);
        }

        public char getChatByIndex(int index)
        {
            return getCharByIndexStringWrapper(anomalyDetector, index);
        }
        public string GetMostCorrelativeFeature(string feature)
        {
            Trace.WriteLine("anomaly file"+getMostCorrelativeFeature(anomalyDetector, new StringBuilder(feature)));
            int strLen = getLenOfStringWrapper(anomalyDetector);
            string s = "";
            for (int i = 0; i < strLen; i++)
            {
                char c = getCharByIndexStringWrapper(anomalyDetector, i);
                s += c.ToString();
            }
            return s;
        }
        //helper function for GetLinearRegression

        public int getLenOfFloatArrayWrapper()
        {
            return getLenOfFloatArrayWrapper(anomalyDetector);
        }

        public float getFloatArrayByIndex(int index)
        {
            return getFloatArrayByIndex(anomalyDetector, index);
        }

        //before using need to run learnNormalFromCSV(StringBuilder CSVfileName)
        public float[] GetLinearRegression(string feature)
        {
            getLinearRegression(anomalyDetector, new StringBuilder(feature));
            int length = getLenOfFloatArrayWrapper();
            float[] line = new float[2];
            for (int i = 0; i < length; i++)
            {
                line[i] = getFloatArrayByIndex(i);
            }
            return line;
        }

        public float[] GetMinCircle(string feature)
        {
            return new float[3]; // todo implement
        }
        
        public int detectFromCSV(StringBuilder CSVfileName)
        {
            return detectFromCSV(anomalyDetector, CSVfileName);
        }

        public int getLenOfArrayWrapper()
        {
            return getLenOfArrayWrapper(anomalyDetector);
        }
        public int getAnomalyByIndex(int index)
        {
            return getAnomalyByIndex(anomalyDetector, index);
        }

        public int[] GetAllAnomaliesTimesSteps()
        {
            getAllAnomalyTimestamp(anomalyDetector);
            int length = getLenOfArrayWrapper();
            int[] anomalies = new int[length];
            for(int i = 0; i < length; i++)
            {
                anomalies[i] = getAnomalyByIndex(i);
            }
            return anomalies;
        }
    }
}
