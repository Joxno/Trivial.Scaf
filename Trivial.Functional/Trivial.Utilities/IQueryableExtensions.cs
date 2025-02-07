using Trivial.Functional;

namespace Trivial.Utilities;

public static class QueryableExtensions
{
    public static Maybe<T> FirstOrNone<T>(this IQueryable<T> Query)
    {
        var t_First = Query.FirstOrDefault();
        if (t_First != null) return (T)t_First;
        return Maybe.None;
    }
}