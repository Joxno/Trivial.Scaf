using System;

namespace Trivial.Functional
{
    public static class ApplyExtensions
    {
        public static Func<TR> Apply<T1, TR>(this Func<T1, TR> Func, 
            T1 Value) =>
            () => Func(Value);

        public static Func<T2, TR> Apply<T1, T2, TR>(this Func<T1, T2, TR> Func, 
            T1 Value) =>
            (P) => Func(Value, P);

        public static Func<T2, TR> Apply<T1, T2, TR>(this Func<T1, Func<T2, TR>> Func, 
            T1 Value) =>
            (P) => Func(Value)(P);

        public static Func<T3, TR> Apply<T1, T2, T3, TR>
            (this Func<T1, Func<T2, Func<T3, TR>>> Func, 
            T1 V1, T2 V2) =>
            (P) => Func(V1)(V2)(P);

        public static Func<T4, TR> Apply<T1, T2, T3, T4, TR>
            (this Func<T1, Func<T2, Func<T3, Func<T4, TR>>>> Func, 
            T1 V1, T2 V2, T3 V3) =>
            (P) => Func(V1)(V2)(V3)(P);

        public static Func<T5, TR> Apply<T1, T2, T3, T4, T5, TR>
            (this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TR>>>>> Func, 
            T1 V1, T2 V2, T3 V3, T4 V4) =>
            (P) => Func(V1)(V2)(V3)(V4)(P);

        public static Func<T6, TR> Apply<T1, T2, T3, T4, T5, T6, TR>
            (this Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, TR>>>>>> Func, 
            T1 V1, T2 V2, T3 V3, T4 V4, T5 V5) =>
            (P) => Func(V1)(V2)(V3)(V4)(V5)(P);
    }
}