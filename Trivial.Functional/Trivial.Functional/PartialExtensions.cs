using System;

namespace Trivial.Functional
{
    public static class PartialExtensions
    {
        public static Func<T2, TR> Partial<T1, T2, TR>(this Func<T1, T2, TR> Func, T1 Value) =>
            (P) => Func(Value, P);

        public static Func<T3, TR> Partial<T1, T2, T3, TR>(this Func<T1, T2, T3, TR> Func, T1 V1, T2 V2) =>
            (P) => Func(V1, V2, P);
    }
}