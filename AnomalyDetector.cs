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
        private string _csvPath;
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
            this._csvPath = csvPath;
            this.DetectorType = detectorType;
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                this.anomalyDetector = create(); //create simple anomaly detector
            }
            else
            {
                //this.anomalyDetector = null;  //todo
            }
        }
        public int learnNormalFromCSV(StringBuilder CSVfileName)
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                return learnNormalFromCSV(anomalyDetector, CSVfileName);
            }
            else
            {
                return 1;
            }
        }
        public int getLenOfStringBuilder()
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                return getLenOfStringWrapper(anomalyDetector);
            }
            else
            {
                return 1;
            }
        }

        public char getChatByIndex(int index)
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                return getCharByIndexStringWrapper(anomalyDetector, index);
            }
            else
            {
                return 'a';
            }
        }
        public string GetMostCorrelativeFeature(string feature)
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                Trace.WriteLine("anomaly file" + getMostCorrelativeFeature(anomalyDetector, new StringBuilder(feature)));
                int strLen = getLenOfStringWrapper(anomalyDetector);
                string s = "";
                for (int i = 0; i < strLen; i++)
                {
                    char c = getCharByIndexStringWrapper(anomalyDetector, i);
                    s += c.ToString();
                }
                return s;
            }
            else
            {
                return "hello";
            }
        }
        //helper function for GetLinearRegression

        public int getLenOfFloatArrayWrapper()
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                return getLenOfFloatArrayWrapper(anomalyDetector);
            }
            else
            {
                return 1;
            }
        }

        public float getFloatArrayByIndex(int index)
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                return getFloatArrayByIndex(anomalyDetector, index);
            }
            else
            {
                return 1;
            }
        }

        //before using need to run learnNormalFromCSV(StringBuilder CSVfileName)
        public float[] GetLinearRegression(string feature)
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
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
            else
            {
                return null;
            }
        }

        public float[] GetMinCircle(string feature)
        {
            return new float[3]; // todo implement
        }

        public int detectFromCSV(StringBuilder CSVfileName)
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                return detectFromCSV(anomalyDetector, CSVfileName);
            }
            else
            {
                return 0;
            }
        }

        public int getLenOfArrayWrapper()
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                return getLenOfArrayWrapper(anomalyDetector);
            }
            else
            {
                return 1;
            }
        }
        public int getAnomalyByIndex(int index)
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                return getAnomalyByIndex(anomalyDetector, index);
            }
            else
            {
                return 1;
            }
        }

        public int[] GetAllAnomaliesTimesSteps()
        {
            if (this.DetectorType.Equals(AnomalyDetectorType.LinearRegression))
            {
                getAllAnomalyTimestamp(anomalyDetector);
                int length = getLenOfArrayWrapper();
                int[] anomalies = new int[length];
                for (int i = 0; i < length; i++)
                {
                    anomalies[i] = getAnomalyByIndex(i);
                }
                return anomalies;
            }
            else
            {
                return null;
            }
        }
    }
}

