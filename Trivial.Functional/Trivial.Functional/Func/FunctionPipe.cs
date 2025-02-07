using System;
using System.Linq;

namespace Trivial.Functional
{
    public static partial class Function
    {
        public static Func<T2> Pipe<T1, T2>(Func<T1> A, Func<T1, T2> B) =>
            () => B(A());

        public static Func<T1, T3> Pipe<T1, T2, T3>(Func<T1, T2> A, Func<T2, T3> B) =>
            (P) => B(A(P));

        public static Func<T1, TR> Pipe<T1, T2, T3, TR>(Func<T1, T2> A, Func<T2, T3> B, Func<T3, TR> C) =>
            (P) => C(B(A(P)));

        public static Func<T1, TR> Pipe<T1, T2, T3, T4, TR>
            (Func<T1, T2> A, Func<T2, T3> B, Func<T3, T4> C, Func<T4, TR> D) =>
            (P) => D(C(B(A(P))));

        public static Func<T1, TR> Pipe<T1, T2, T3, T4, T5, TR>
            (Func<T1, T2> A, Func<T2, T3> B, Func<T3, T4> C, Func<T4, T5> D, Func<T5, TR> E) =>
            (P) => E(D(C(B(A(P)))));

        public static Func<T1, TR> Pipe<T1, T2, T3, T4, T5, T6, TR>
            (Func<T1, T2> A, Func<T2, T3> B, Func<T3, T4> C, Func<T4, T5> D, Func<T5, T6> E, Func<T6, TR> F) =>
            (P) => F(E(D(C(B(A(P))))));

        public static Func<T1, TR> Pipe<T1, T2, T3, T4, T5, T6, T7, TR>
            (Func<T1, T2> A, Func<T2, T3> B, Func<T3, T4> C, Func<T4, T5> D, 
            Func<T5, T6> E, Func<T6, T7> F, Func<T7, TR> G) =>
            (P) => G(F(E(D(C(B(A(P)))))));

        public static Func<T, T> Pipe<T>(params Func<T, T>[] Funcs) =>
            Funcs.Length < 2 ? (Funcs.Length == 1 ? Funcs[0] : (P) => P) :
            Functions.Let(Funcs.First(), F => 
                Funcs.Skip(1).Aggregate(F, (A, B) => Pipe(A, B)))();

        public static Func<T1, T3> PipeTo<T1, T2, T3>(this Func<T1, T2> A, Func<T2, T3> B) =>
            (P) => B(A(P));

        //public static Func<T> PipeTo<T>(this Func<T> A, Func<T, T> B) =>
        //    () => B(A());

        public static Func<TResult> PipeTo<T, TResult>(this Func<T> A, Func<T, TResult> B) =>
            () => B(A());

        public static Func<T1, T2> Pipe<T1, T2>(Func<T1, T2> A, Action<T2> B) =>
            (P) => Tap(B)(A(P));

        public static Func<T1, T2> Pipe<T1, T2>(Action<T1> A, Func<T1, T2> B) =>
            (P) => B(Tap(A)(P));

        public static Func<T1, T2> Pipe<T1, T2>(Func<T1, T1> Id, Func<T2> A) =>
            (P) => { Id(P); return A(); };

        public static Func<T, T> Tap<T, T2>(Func<T, T2> Func) =>
            (P) => { Func(P); return P; };

        public static Func<T, T> Tap<T>(Action<T> Func) =>
            (P) => { Func(P); return P; };

        public static Func<TResult> Tap<TResult>(this Func<TResult> Func, Action<TResult> TapFunc) =>
            () => Functions.Let(Func(), R => {
                TapFunc(R);
                return R;
            })();

        public static Func<T, TResult> Tap<T, TResult>(this Func<T, TResult> Func, Action<TResult> TapFunc) =>
            (P) => Functions.Let(Func(P), R => {
                TapFunc(R);
                return R;
            })();

        public static Func<T1, T2, TResult> Tap<T1, T2, TResult>(this Func<T1, T2, TResult> Func, Action<TResult> TapFunc) =>
            (P1, P2) => Functions.Let(Func(P1, P2), R => {
                TapFunc(R);
                return R;
            })();

        public static Func<T1, T2, T3, TResult> Tap<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> Func, Action<TResult> TapFunc) =>
            (P1, P2, P3) => Functions.Let(Func(P1, P2, P3), R => {
                TapFunc(R);
                return R;
            })();

        public static Func<T1, T2, T3, T4, TResult> Tap<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> Func, Action<TResult> TapFunc) =>
            (P1, P2, P3, P4) => Functions.Let(Func(P1, P2, P3, P4), R => {
                TapFunc(R);
                return R;
            })();
    }
}