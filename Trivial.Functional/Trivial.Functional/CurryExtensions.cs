using System;

namespace Trivial.Functional
{
    public static class CurryExtensions
    {
        public static Func<T1, Func<T2, TR>> 
            Curry<T1, T2, TR>
            (this Func<T1, T2, TR> Func) =>
            P1 => P2 => Func(P1, P2);

        public static Func<T1, Func<T2, Func<T3, TR>>> 
            Curry<T1, T2, T3, TR>
            (this Func<T1, T2, T3, TR> Func) =>
            P1 => P2 => P3 => Func(P1, P2, P3);

        public static Func<T1, Func<T2, Func<T3, Func<T4, TR>>>> 
            Curry<T1, T2, T3, T4, TR>
            (this Func<T1, T2, T3, T4, TR> Func) =>
            P1 => P2 => P3 => P4 => Func(P1, P2, P3, P4);

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, TR>>>>> 
            Curry<T1, T2, T3, T4, T5, TR>
            (this Func<T1, T2, T3, T4, T5, TR> Func) =>
            P1 => P2 => P3 => P4 => P5 => Func(P1, P2, P3, P4, P5);

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, TR>>>>>> 
            Curry<T1, T2, T3, T4, T5, T6, TR>
            (this Func<T1, T2, T3, T4, T5, T6, TR> Func) =>
            P1 => P2 => P3 => P4 => P5 => P6 => Func(P1, P2, P3, P4, P5, P6);

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, TR>>>>>>>
            Curry<T1, T2, T3, T4, T5, T6, T7, TR>
            (this Func<T1, T2, T3, T4, T5, T6, T7, TR> Func) =>
            P1 => P2 => P3 => P4 => P5 => P6 => P7 => Func(P1, P2, P3, P4, P5, P6, P7);

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, TR>>>>>>>>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, TR>
            (this Func<T1, T2, T3, T4, T5, T6, T7, T8, TR> Func) =>
            P1 => P2 => P3 => P4 => P5 => P6 => P7 => P8 => Func(P1, P2, P3, P4, P5, P6, P7, P8);

        public static Func<T1, Func<T2, Func<T3, Func<T4, Func<T5, Func<T6, Func<T7, Func<T8, Func<T9, TR>>>>>>>>>
            Curry<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR>
            (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TR> Func) =>
            P1 => P2 => P3 => P4 => P5 => P6 => P7 => P8 => P9 => Func(P1, P2, P3, P4, P5, P6, P7, P8, P9);


        public static Func<T2, Func<T1, TResult>> CurryReverse<T1, T2, TResult>(this Func<T1, T2, TResult> Func) =>
            (T2 P2) => (T1 P1) => Func(P1, P2);

        public static Func<T3, Func<T2, Func<T1, TResult>>> CurryReverse<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> Func) =>
            (T3 P3) => (T2 P2) => (T1 P1) => Func(P1, P2, P3);

        public static Func<T4, Func<T3, Func<T2, Func<T1, TResult>>>> CurryReverse<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> Func) =>
            (T4 P4) => (T3 P3) => (T2 P2) => (T1 P1) => Func(P1, P2, P3, P4);

        public static Func<T5, Func<T4, Func<T3, Func<T2, Func<T1, TResult>>>>> CurryReverse<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> Func) =>
            (T5 P5) => (T4 P4) => (T3 P3) => (T2 P2) => (T1 P1) => Func(P1, P2, P3, P4, P5);

        public static Func<T6, Func<T5, Func<T4, Func<T3, Func<T2, Func<T1, TResult>>>>>> CurryReverse<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> Func) =>
            (T6 P6) => (T5 P5) => (T4 P4) => (T3 P3) => (T2 P2) => (T1 P1) => Func(P1, P2, P3, P4, P5, P6);

        public static Func<T7, Func<T6, Func<T5, Func<T4, Func<T3, Func<T2, Func<T1, TResult>>>>>>> CurryReverse<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> Func) =>
            (T7 P7) => (T6 P6) => (T5 P5) => (T4 P4) => (T3 P3) => (T2 P2) => (T1 P1) => Func(P1, P2, P3, P4, P5, P6, P7);

    }
}