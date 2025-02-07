using System;
using Trivial.Functional;

namespace Trivial.Utilities
{
    public static class ResultExtensions
    {
        public static void Then<T>(this Result<T> E, Action<T> ThenMethod)
        {
            if (E.HasValue)
                ThenMethod?.Invoke(E.Value);
        }

        public static Result<T2> Then<T1, T2>(this Result<T1> E, Func<T1, T2> ThenMethod)
        {
            if (E.HasValue)
                return Try.Invoke(ThenMethod, E.Value);

            return new Result<T2>(E.Error);
        }

        public static void Then<T>(this Result<T> R, Action<T> ThenMethod, Action<Exception> ErrorMethod)
        {
            if (R.HasValue)
                ThenMethod?.Invoke(R.Value);
            else
                ErrorMethod?.Invoke(R.Error);
        }

        public static void Then<T1, T2>(this ValueTuple<Result<T1>, Result<T2>> Tuple, Action<T1, T2> ThenMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue)
                ThenMethod?.Invoke(Tuple.Item1.Value, Tuple.Item2.Value);
        }

        public static Result<TResult> Then<T1, T2, TResult>(this ValueTuple<Result<T1>, Result<T2>> Tuple, Func<T1, T2, TResult> ThenMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue)
                return ThenMethod.Invoke(Tuple.Item1.Value, Tuple.Item2.Value);

            if(Tuple.Item1.HasError) return Tuple.Item1.Error;
            if(Tuple.Item2.HasError) return Tuple.Item2.Error;

            return new Exception("No value or error was returned from the ThenMethod.");
        }

        public static Result<TResult> Then<T1, T2, T3, TResult>(this ValueTuple<Result<T1>, Result<T2>, Result<T3>> Tuple, Func<T1, T2, T3, TResult> ThenMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue && Tuple.Item3.HasValue)
                return ThenMethod.Invoke(Tuple.Item1.Value, Tuple.Item2.Value, Tuple.Item3.Value);

            if(Tuple.Item1.HasError) return Tuple.Item1.Error;
            if(Tuple.Item2.HasError) return Tuple.Item2.Error;
            if(Tuple.Item3.HasError) return Tuple.Item3.Error;

            return new Exception("No value or error was returned from the ThenMethod.");
        }

        public static void ThenOrElse<T1, T2>(this ValueTuple<Result<T1>, Result<T2>> Tuple, Action<T1, T2> ThenMethod, Action<Exception> ElseT1Method, Action<Exception> ElseT2Method)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue)
                ThenMethod?.Invoke(Tuple.Item1.Value, Tuple.Item2.Value);
            else if(Tuple.Item1.HasError)
                ElseT1Method?.Invoke(Tuple.Item1.Error);
            else if(Tuple.Item2.HasError)
                ElseT2Method?.Invoke(Tuple.Item2.Error);
        }

        public static Result<T2> ThenOrElse<T1, T2>(this Result<T1> R, Func<T1, T2> ThenMethod, Func<Exception, T2> ElseMethod)
        {
            if (R.HasValue)
                return Try.Invoke(ThenMethod, R.Value);

            return Try.Invoke(ElseMethod, R.Error);
        }

        public static void ThenOrElse<T>(this Result<T> R, Action<T> ThenMethod, Action<Exception> ElseMethod)
        {
            if (R.HasValue)
                ThenMethod?.Invoke(R.Value);
            else
                ElseMethod?.Invoke(R.Error);
        }

        public static void Then<T1, T2, T3>(this ValueTuple<Result<T1>, Result<T2>, Result<T3>> Tuple, Action<T1, T2, T3> ThenMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue && Tuple.Item3.HasValue)
                ThenMethod?.Invoke(Tuple.Item1.Value, Tuple.Item2.Value, Tuple.Item3.Value);
        }

        public static void Then<T1, T2, T3, T4>(this ValueTuple<Result<T1>, Result<T2>, Result<T3>, Result<T4>> Tuple, Action<T1, T2, T3, T4> ThenMethod)
        {
            if(Tuple.Item1.HasValue && Tuple.Item2.HasValue && Tuple.Item3.HasValue && Tuple.Item4.HasValue)
                ThenMethod?.Invoke(Tuple.Item1.Value, Tuple.Item2.Value, Tuple.Item3.Value, Tuple.Item4.Value);
        }

        public static Result<T> Flatten<T>(this Result<Result<T>> R) =>
            R.HasError ? R.Error : R.Value;

        public static Result<T> ToResult<T>(this T Value) => Value;
        public static Result<T> ToResult<T>(this Maybe<T> M) => M.HasValue ? M.Value : new NullReferenceException("Maybe<T> was null when converting to Result<T>");
        public static Result<T> ToResult<T>(this Maybe<T> M, Exception OnNull) => M.HasValue ? M.Value : OnNull;

        public static Result<T> OnError<T>(this Result<T> R, Action<Exception> ErrorMethod)
        {
            if (R.HasError)
                ErrorMethod?.Invoke(R.Error);

            return R;
        }

        public static Result<T> OnValue<T>(this Result<T> R, Action<T> SuccessMethod)
        {
            if (R.HasValue)
                SuccessMethod?.Invoke(R.Value);

            return R;
        }

        public static Result<T> Chain<T>(this Result<T> R, Func<T, Result<Unit>> ChainMethod)
        {
            if (R.HasValue)
            {
                var t_Chain = ChainMethod.Invoke(R.Value);
                if (t_Chain.HasError)
                    return t_Chain.Error;
            }

            return R;
        }

        public static Result<T> Chain<T>(this Result<T> R, Action<T> ChainMethod) =>
            R.Chain(C => Try.Invoke(ChainMethod, C));

        public static T FromResult<T>(this Result<T> R) =>
            R.HasValue ? R.Value : throw R.Error;

        public static T FromResult<T>(this Result<T> R, Func<T> Default) =>
            R.HasValue ? R.Value : Default();

        public static T ValueOr<T>(this Result<T> M, T Default) =>
            M.HasValue ? M.Value : Default;

        public static T2 ValueOr<T1, T2>(this Result<T1> M, Func<T1, T2> ValueSelector, T2 Default) =>
            M.HasValue ? ValueSelector(M.Value) : Default;

        public static Result<T2> MapResult<T1, T2>(this T1 Val, Func<T1, T2> MapFunc) =>
            Val.ToMaybe().ToResult().Map(MapFunc);

        public static Result<Unit> ToUnit<T>(this Result<T> R) => R.HasValue ? Defaults.Unit : R.Error;
        public static Task<Result<Unit>> ToUnitAsync<T>(this Task<Result<T>> R) => 
            R.ContinueWith<Result<Unit>>(T => {
                if(!T.Exception.IsNull())
                    return T.Exception!;

                return T.Result.HasValue ? Defaults.Unit : T.Result.Error;
            });
    }
}
