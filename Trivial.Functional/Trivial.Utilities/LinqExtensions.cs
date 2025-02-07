using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trivial.Functional;
using Trivial.Utilities;

namespace Trivial.Utilities;

public static class LinqExtensions
{
    public static IEnumerable<T> FilterNulls<T>(this IEnumerable<T> Source) where T : class =>
        Source.Where(Item => !Item.IsNull());

    public static List<T> FilterNulls<T>(this T[] Source) =>
        Source.Where(Item => !Item.IsNull()).ToList();

    public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> Source) =>
        Source.Count() > 0 ? Source.First() : Maybe.None;

    public static Maybe<T> FirstOrNone<T>(this IEnumerable<T> Source, Func<T, bool> Predicate) =>
        Source.Any(Predicate) ? Source.First(Predicate).ToMaybe() : Maybe.None;

    public static bool IsNullOrEmpty<T>(this T[] Source) =>
        Source.IsNull() || Source.Length == 0;

    public static List<T> ToListOrEmpty<T>(this T[] Source) =>
        Source.IsNullOrEmpty() ? new List<T>() : Source.FilterNulls();
}