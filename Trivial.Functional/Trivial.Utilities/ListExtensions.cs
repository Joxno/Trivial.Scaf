using System.Runtime.InteropServices;

namespace Trivial.Utilities;

public static class ListExtensions
{
    public static void Deconstruct<T>(this IList<T> List, out T First, out IList<T> Rest) {
        First = List.Count > 0 ? List[0] : default(T); // or throw
        Rest = List.Skip(1).ToList();
    }

    public static void Deconstruct<T>(this IList<T> List, out T First, out T Second) {
        First = List.Count > 0 ? List[0] : default(T); // or throw
        Second = List.Count > 1 ? List[1] : default(T); // or throw
    }

    public static void Deconstruct<T>(this IList<T> List, out T First, out T Second, out T Third) {
        First = List.Count > 0 ? List[0] : default(T); // or throw
        Second = List.Count > 1 ? List[1] : default(T); // or throw
        Third = List.Count > 2 ? List[2] : default(T);
    }

    public static IEnumerable<T> ToIEnumerable<T>(this T Object) =>
        new List<T>() { Object };

    public static IEnumerable<T> Resolve<T>(this IEnumerable<T> Object) =>
        Object.ToList();

    public static IEnumerable<IEnumerable<T>> Resolve<T>(this IEnumerable<IEnumerable<T>> Object) =>
        Object.Select(I => I.Resolve()).Resolve();

    // public static Span<T> AsSpan<T>(this List<T> List) =>
    //     CollectionsMarshal.AsSpan(List);
    public static Span<T> AsSpan<T>(this List<T> List)
    {
        return CollectionsMarshal.AsSpan(List);
    }
}