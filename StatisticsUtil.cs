using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    static class StatisticsUtil
    {
        public static double Expectancy(float[] xArray)
        {
            // Todo Implement
            return 0;
        }

        public static float Variance(float[] xArray)
        {
            // Todo Implement
            float expectancy = xArray.Average();
            return 0;
        }

        public static float Covariance(float[] xArray, float[] yArray)
        {
            // Todo Implement
            return 0;
        }

        public static float Pearson(float[] xArray, float[] yArray)
        {
            // Todo Implement
            return 0;
        }

        private static float[] SquareAllElements(float[] xArray)
        {
            float[] res = new float[xArray.Length];
            for (int i = 0; i < xArray.Length; i++)
            {
                res[i] = Math.Pow(xArray[i], 2);
            }
        }
    }
}
