using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightDetector
{
    static class StatisticsUtil
    {
        public static double Expectancy(double[] xArray)
        {
            // Todo Implement
            return 0;
        }

        public static double Variance(double[] xArray)
        {
            // Todo Implement
            double expectancy = xArray.Average();
            double[] squared = SquareAllElements(xArray);
            double squaredExpectancy = squared.Average();
            return squaredExpectancy - Math.Pow(expectancy, 2);
        }

        public static double Covariance(double[] xArray, double[] yArray)
        {
            // Todo Implement
            return 0;
        }

        public static double Pearson(double[] xArray, double[] yArray)
        {
            // Todo Implement
            return 0;
        }

        private static double[] SquareAllElements(double[] xArray)
        {
            double[] res = new double[xArray.Length];
            for (int i = 0; i < xArray.Length; i++)
            {
                res[i] = Math.Pow(xArray[i], 2);
            }

            return res;
        }
    }
}
