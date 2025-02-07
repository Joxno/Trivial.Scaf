using System;
using System.Collections.Generic;
//using System.Collections.Immutable;
using System.Linq;
using Trivial.Functional;

namespace Trivial.Utilities
{
    public static class MaybeExtensions
    {
        public static void Then<T>(this Maybe<T> M, Action<T> ThenMethod)
        {
            if (M.HasValue)
                ThenMethod?.Invoke(M.Value);
        }

        public static void Then<T1, T2>(this ValueTuple<Maybe<T1>, Maybe<T2>> Tuple, Action<T1, T2> ThenMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue)
                ThenMethod?.Invoke(Tuple.Item1.Value, Tuple.Item2.Value);
        }

        public static void Then<T1, T2, T3>(this ValueTuple<Maybe<T1>, Maybe<T2>, Maybe<T3>> Tuple, Action<T1, T2, T3> ThenMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue && Tuple.Item3.HasValue)
                ThenMethod?.Invoke(Tuple.Item1.Value, Tuple.Item2.Value, Tuple.Item3.Value);
        }

        public static void Then<T1, T2, T3, T4>(this ValueTuple<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>> Tuple, Action<T1, T2, T3, T4> ThenMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue && Tuple.Item3.HasValue && Tuple.Item4.HasValue)
                ThenMethod?.Invoke(Tuple.Item1.Value, Tuple.Item2.Value, Tuple.Item3.Value, Tuple.Item4.Value);
        }

        public static Maybe<TResult> Then<T1, T2, TResult>(this ValueTuple<Maybe<T1>, Maybe<T2>> Tuple, Func<T1, T2, TResult> ThenMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue)
                return ThenMethod.Invoke(Tuple.Item1.Value, Tuple.Item2.Value);

            return Maybe.None;
        }

        public static Maybe<TResult> Then<T1, T2, T3, TResult>(this ValueTuple<Maybe<T1>, Maybe<T2>, Maybe<T3>> Tuple, Func<T1, T2, T3, TResult> ThenMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue && Tuple.Item3.HasValue)
                return ThenMethod.Invoke(Tuple.Item1.Value, Tuple.Item2.Value, Tuple.Item3.Value);

            return Maybe.None;
        }

        public static void ThenOrElse<T1, T2>(this ValueTuple<Maybe<T1>, Maybe<T2>> Tuple, Action<T1, T2> ThenMethod, Action ElseMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue)
                ThenMethod?.Invoke(Tuple.Item1.Value, Tuple.Item2.Value);
            else
                ElseMethod?.Invoke();
        }

        public static TResult ThenOrElse<T1, T2, TResult>(this ValueTuple<Maybe<T1>, Maybe<T2>> Tuple, Func<T1, T2, TResult> ThenMethod, Func<TResult> ElseMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue)
                return ThenMethod.Invoke(Tuple.Item1.Value, Tuple.Item2.Value);
            else
                return ElseMethod.Invoke();
        }

        public static void ThenOrElse<T1, T2, T3>(this ValueTuple<Maybe<T1>, Maybe<T2>, Maybe<T3>> Tuple, Action<T1, T2, T3> ThenMethod, Action ElseMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue && Tuple.Item3.HasValue)
                ThenMethod?.Invoke(Tuple.Item1.Value, Tuple.Item2.Value, Tuple.Item3.Value);
            else
                ElseMethod?.Invoke();
        }

        public static void ThenOrElse<T1, T2, T3, T4>(this ValueTuple<Maybe<T1>, Maybe<T2>, Maybe<T3>, Maybe<T4>> Tuple, Action<T1, T2, T3, T4> ThenMethod, Action ElseMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue && Tuple.Item3.HasValue && Tuple.Item4.HasValue)
                ThenMethod?.Invoke(Tuple.Item1.Value, Tuple.Item2.Value, Tuple.Item3.Value, Tuple.Item4.Value);
            else
                ElseMethod?.Invoke();
        }

        public static void ThenOrElse<T>(this Maybe<T> M, Action<T> ThenMethod, Action ElseMethod)
        {
            if (M.HasValue)
                ThenMethod?.Invoke(M.Value);
            else
                ElseMethod?.Invoke();
        }

        public static Maybe<T2> Then<T1, T2>(this Maybe<T1> M, Func<T1, T2> ThenMethod)
        {
            if (M.HasValue)
                return Try.GetFrom(ThenMethod)(M.Value);

            return new Maybe<T2>();
        }

        public static Maybe<T2> Then<T1, T2>(this Maybe<T1> M, Func<T1, Maybe<T2>> ThenMethod) =>
            M.Bind(ThenMethod);

        public static Maybe<T2> ThenOrElse<T1, T2>(this Maybe<T1> M, Func<T1, T2> ThenMethod, Func<T2> ElseMethod)
        {
            if (M.HasValue)
                return Try.GetFrom(ThenMethod)(M.Value);

            return Try.From(ElseMethod);
        }

        public static void Else<T>(this Maybe<T> M, Action ElseMethod)
        {
            if (!M.HasValue)
                ElseMethod?.Invoke();
        }

        public static void OnNone<T>(this Maybe<T> M, Action ElseMethod) =>
            M.Else(ElseMethod);

        public static T ValueOr<T>(this Maybe<T> M, T Default) =>
            M.HasValue ? M.Value : Default;

        public static T ValueOrLazy<T>(this Maybe<T> M, Func<T> Default) =>
            M.HasValue ? M.Value : Default();

        public static T2 ValueOrLazy<T1, T2>(this Maybe<T1> M, Func<T1, T2> ValueSelector, Func<T2> Default) =>
            M.HasValue ? ValueSelector(M.Value) : Default();

        public static T2 ValueOr<T1, T2>(this Maybe<T1> M, Func<T1, T2> ValueSelector, T2 Default) =>
            M.HasValue ? ValueSelector(M.Value) : Default;

        public static T ValueOrThrow<T>(this Maybe<T> M, Exception E) =>
            M.HasValue ? M.Value : throw E;

        // public static Maybe<T> ThenOrElse<T>(this Maybe<T> M, Func<T, T> ThenMethod, Func<T> ElseMethod)
        // {
        //     if (M.HasValue)
        //         return Try.GetFrom(ThenMethod)(M.Value);

        //     return Try.From(ElseMethod);
        // }

        public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> M) =>
            M.Value;

        public static Maybe<T> ToMaybe<T>(this T Value) => Value;
        public static Maybe<T> ToMaybe<T>(this Result<T> Value) => Value.HasValue ? Value.Value : Maybe.None;
        public static Maybe<T2> AsMaybe<T1, T2>(this T1 Value) 
            where T2 : class
            where T1 : class
            => Value as T2;

        public static Maybe<(T1, T2)> ToMaybe<T1, T2>(this ValueTuple<T1, T2> Tuple) =>
            Tuple.Item1.ToMaybe().HasValue && 
            Tuple.Item2.ToMaybe().HasValue ? 
                (Tuple.Item1, Tuple.Item2) :
                Maybe.None;

        public static Maybe<T> ToMaybe<T>(this IEnumerable<T> Enumerable, Func<T, bool> First) =>
            Enumerable.FirstOrDefault(First);

        public static Result<T> ToResult<T>(this Maybe<T> M, Func<Exception> OnEmpty) => 
            M.HasValue ? M.Value : OnEmpty?.Invoke();

        public static Result<T> ToResult<T>(this Maybe<Result<T>> M, Func<Exception> OnEmpty) => 
            M.HasValue ? M.Value : OnEmpty?.Invoke();

        public static Maybe<T2> Select<T, T2>(this Maybe<T> M, Func<T, T2> Func) =>
            M.Map(Func);

        public static Maybe<T3> SelectMany<T, T2, T3>(this Maybe<T> M, Func<T, Maybe<T2>> Func, Func<T, T2, T3> S) =>
            M.Bind(X => Func(X).Bind(Y => Maybe.Return(S(X, Y))));

        public static Maybe<IEnumerable<T>> Raise<T>(this IEnumerable<Maybe<T>> Enumerable) =>
            Enumerable.All(M => M.HasValue)
                    ? new Maybe<IEnumerable<T>>(Enumerable.Select(M => M.Value))
                    : Maybe.Null;

        public static Maybe<List<T>> Raise<T>(this List<Maybe<T>> Enumerable) =>
            Enumerable.All(M => M.HasValue)
                    ? new Maybe<List<T>>(Enumerable.Select(M => M.Value).ToList())
                    : Maybe.Null;

        public static Maybe<T> Chain<T>(this Maybe<T> R, Action<T> ChainMethod)
        {
            if (R.HasValue)
                ChainMethod?.Invoke(R.Value);

            return R;
        }

        public static Maybe<T2> MapMaybe<T1, T2>(this T1 Val, Func<T1, T2> MapFunc) =>
            Val.ToMaybe().Map(MapFunc);

/*
        public static Maybe<ImmutableList<T>> Raise<T>(this ImmutableList<Maybe<T>> Enumerable) =>
            Enumerable.All(M => M.HasValue)
                    ? new Maybe<ImmutableList<T>>(Enumerable.Select(M => M.Value).ToImmutableList())
                    : Maybe.Null;
*/
    }
}
