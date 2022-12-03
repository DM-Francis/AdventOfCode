using System;
using System.Collections.Generic;
using System.Text;

namespace Day10_MonitoringStation
{
    public static class NumericHelper
    {
        public static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public static int AbsGCD(int a, int b)
        {
            return Math.Abs(GCD(a, b));
        }
    }
}
