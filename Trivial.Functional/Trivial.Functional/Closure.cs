using System;
using ClosureClass = Trivial.Functional.Closure;

namespace Trivial.Functional
{
    public static class Closure
    {
        public static Action Create(Action Closure) => Closure;
        public static Action Create<T>(T Arg, Action<T> Closure) => () => Closure(Arg);
        public static Action Create<T1, T2>(T1 Arg1, T2 Arg2, Action<T1, T2> Closure) => () => Closure(Arg1, Arg2);
        public static Action Create<T1, T2, T3>(T1 Arg1, T2 Arg2, T3 Arg3, Action<T1, T2, T3> Closure) => () => Closure(Arg1, Arg2, Arg3);
        public static Action Create<T1, T2, T3, T4>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, Action<T1, T2, T3, T4> Closure) => () => Closure(Arg1, Arg2, Arg3, Arg4);
        public static Action Create<T1, T2, T3, T4, T5>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, Action<T1, T2, T3, T4, T5> Closure) => () => Closure(Arg1, Arg2, Arg3, Arg4, Arg5);
        public static Action Create<T1, T2, T3, T4, T5, T6>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, Action<T1, T2, T3, T4, T5, T6> Closure) => () => Closure(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6);
        public static Action Create<T1, T2, T3, T4, T5, T6, T7>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, Action<T1, T2, T3, T4, T5, T6, T7> Closure) => () => Closure(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7);
        public static Action Create<T1, T2, T3, T4, T5, T6, T7, T8>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, Action<T1, T2, T3, T4, T5, T6, T7, T8> Closure) => () => Closure(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8);
        public static Action Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> Closure) => () => Closure(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9);
        public static Action Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, T10 Arg10, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Closure) => () => Closure(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10);
        public static Action Create<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, T10 Arg10, T11 Arg11, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Closure) => () => Closure(Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Arg11);
    }

    public static class ClosureExtensions
    {
        public static Action Closure<TThis>(this TThis This, Action<TThis> C) => ClosureClass.Create(This, C);
        public static Action Closure<TThis, T1>(this TThis This, T1 Arg, Action<TThis, T1> Closure) => ClosureClass.Create(This, Arg, Closure);
        public static Action Closure<TThis, T1, T2>(this TThis This, T1 Arg1, T2 Arg2, Action<TThis, T1, T2> Closure) => ClosureClass.Create(This, Arg1, Arg2, Closure);
        public static Action Closure<TThis, T1, T2, T3>(this TThis This, T1 Arg1, T2 Arg2, T3 Arg3, Action<TThis, T1, T2, T3> Closure) => ClosureClass.Create(This, Arg1, Arg2, Arg3, Closure);
        public static Action Closure<TThis, T1, T2, T3, T4>(this TThis This, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, Action<TThis, T1, T2, T3, T4> Closure) => ClosureClass.Create(This, Arg1, Arg2, Arg3, Arg4, Closure);
        public static Action Closure<TThis, T1, T2, T3, T4, T5>(this TThis This, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, Action<TThis, T1, T2, T3, T4, T5> Closure) => ClosureClass.Create(This, Arg1, Arg2, Arg3, Arg4, Arg5, Closure);
        public static Action Closure<TThis, T1, T2, T3, T4, T5, T6>(this TThis This, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, Action<TThis, T1, T2, T3, T4, T5, T6> Closure) => ClosureClass.Create(This, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Closure);
        public static Action Closure<TThis, T1, T2, T3, T4, T5, T6, T7>(this TThis This, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, Action<TThis, T1, T2, T3, T4, T5, T6, T7> Closure) => ClosureClass.Create(This, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Closure);
        public static Action Closure<TThis, T1, T2, T3, T4, T5, T6, T7, T8>(this TThis This, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, Action<TThis, T1, T2, T3, T4, T5, T6, T7, T8> Closure) => ClosureClass.Create(This, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Closure);
        public static Action Closure<TThis, T1, T2, T3, T4, T5, T6, T7, T8, T9>(this TThis This, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, Action<TThis, T1, T2, T3, T4, T5, T6, T7, T8, T9> Closure) => ClosureClass.Create(This, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Closure);
        public static Action Closure<TThis, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this TThis This, T1 Arg1, T2 Arg2, T3 Arg3, T4 Arg4, T5 Arg5, T6 Arg6, T7 Arg7, T8 Arg8, T9 Arg9, T10 Arg10, Action<TThis, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Closure) => ClosureClass.Create(This, Arg1, Arg2, Arg3, Arg4, Arg5, Arg6, Arg7, Arg8, Arg9, Arg10, Closure);
    }
}