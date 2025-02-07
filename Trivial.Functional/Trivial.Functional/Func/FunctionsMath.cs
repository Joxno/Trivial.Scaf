using System;

namespace Trivial.Functional
{
    public static partial class Functions
    {
        public static class Math
        {
            public static Func<int, int, int> AddInt() =>
            (P1, P2) => P1 + P2;

            public static Func<int, int, int> SubInt() =>
                (P1, P2) => P1 - P2;

            public static Func<int, int, int> DivInt() =>
                (P1, P2) => P1 / P2;

            public static Func<int, int, int> MulInt() =>
                (P1, P2) => P1 * P2;

            public static Func<int, int, int> MaxInt() =>
                (P1, P2) => System.Math.Max(P1, P2);

            public static Func<float, float, float> MaxFloat() =>
                (P1, P2) => System.Math.Max(P1, P2);

            public static Func<double, double, double> MaxDouble() =>
                (P1, P2) => System.Math.Max(P1, P2);

            public static Func<int, int, int> MinInt() =>
                (P1, P2) => System.Math.Min(P1, P2);

            public static Func<float, float, float> MinFloat() =>
                (P1, P2) => System.Math.Min(P1, P2);

            public static Func<double, double, double> MinDouble() =>
                (P1, P2) => System.Math.Min(P1, P2);

            public static Func<double, double> FloorDouble() =>
                (P) => System.Math.Floor(P);

            public static Func<decimal, decimal> FloorDecimal() =>
                (P) => System.Math.Floor(P);

            public static Func<double, double> CeilingDouble() =>
                (P) => System.Math.Ceiling(P);

            public static Func<decimal, decimal> CeilingDecimal() =>
                (P) => System.Math.Ceiling(P);
        }
    }
}