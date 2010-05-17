using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlgTest
{
    class MathUtils
    {
        public static double GetAverage(double[] data)
        {
            int len = data.Length;

            if (len == 0)
            {
                return 0;
            }

            double sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum / len;
        }
    }
}
