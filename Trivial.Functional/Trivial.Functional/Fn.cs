using System;

namespace Trivial.Functional
{
    public static partial class Functions
    {
        public static Func<T> Fn<T>(T Value) => () => Value;
        public static Func<T> Fn<T>(Func<T> F) => () => F();
        public static Func<T> ToFn<T>(this T Value) => () => Value;

        public static T Continue<T>(this T Value, Action<T> ContinueWith) =>
            Value.ToFn().Tap(V => ContinueWith?.Invoke(V))();

        public static T Tap<T>(this T Value, Action<T> ContinueWith) =>
            Value.ToFn().Tap(V => ContinueWith?.Invoke(V))();

        public static T Tap<T, _>(this T Value, Func<T, _> ContinueWith) =>
            Value.ToFn().Tap(V => ContinueWith?.Invoke(V))();

        public static T Mutate<T>(this T Value, Func<T, T> Mutator) =>
            Mutator(Value);

        public static T2 Mutate<T1, T2>(this T1 Value, Func<T1, T2> Mutator) =>
            Mutator(Value);
    }
}