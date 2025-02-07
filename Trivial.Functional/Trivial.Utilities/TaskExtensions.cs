using Trivial.Functional;

namespace Trivial.Utilities;

public static class TaskExtensions
{
    public static Task<T2> Then<T1, T2>(this Task<T1> Task, Func<T1, T2> ThenFunc) =>
        Task.ContinueWith(T => ThenFunc(T.Result), TaskContinuationOptions.OnlyOnRanToCompletion);

    public static Task<Result<T2>> Map<T1, T2>(this Task<Result<T1>> Task, Func<T1, T2> ThenFunc) =>
        Task.ContinueWith(T => T.Result.Map(ThenFunc), TaskContinuationOptions.OnlyOnRanToCompletion);
}