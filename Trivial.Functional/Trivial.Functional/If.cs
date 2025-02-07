using System;
using Trivial.Utilities;

namespace Trivial.Functional
{
    public static partial class Functions
    {
        public static void If(this Func<bool> F, Action OnTrue, Action OnFalse = null) =>
            Try.From(F).Then(B => B ? Try.Invoke(OnTrue) : Try.Invoke(OnFalse));

        public static Result<Unit> If(this bool B, Action OnTrue, Action OnFalse = null) =>
            B ? Try.Invoke(OnTrue) : Try.Invoke(OnFalse);

        public static Action<T> If<T>(Func<bool> F, Action<T> OnTrue, Action<T> OnFalse = null) =>
            (P) => Try.From(F).Then(B => B ? Try.Invoke(OnTrue, P) : Try.Invoke(OnFalse, P));
    }
}