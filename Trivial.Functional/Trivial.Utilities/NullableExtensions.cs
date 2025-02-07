using Trivial.Functional;

namespace Trivial.Utilities;

public static class NullableExtensions
{
    public static Maybe<T> ToMaybe<T>(this System.Nullable<T> Nullable) where T : struct =>
        Nullable.HasValue ? Nullable.Value : Maybe.None;
}