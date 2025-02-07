using System;
using System.Collections.Generic;
using System.Linq;
using Trivial.Functional;

namespace Trivial.Utilities
{
    public static class EnumerableExtensions
    {
        public static string StringJoin<T>(this IEnumerable<T> Collection, string Separator = "") =>
            string.Join(Separator, Collection);

        public static IEnumerable<T> ResolveMaybes<T>(this IEnumerable<Maybe<T>> Collection) =>
            Collection.Where(M => M.HasValue).Select(M => M.Value);

        public static IEnumerable<T> ResolveResults<T>(this IEnumerable<Result<T>> Collection) =>
            Collection.Where(R => R.HasValue).Select(R => R.Value);

        public static Stack<T> ToStack<T>(this IEnumerable<T> Collection, bool FirstElementIsTop = true) =>
            new Stack<T>(FirstElementIsTop ? Collection.Reverse() : Collection);

        public static Result<IEnumerable<T>> InvertResult<T>(this IEnumerable<Result<T>> Collection) =>
            Collection.Any(R => R.HasError) ? Collection.First(R => R.HasError).Error : Collection.Select(R => R.Value).ToResult();

        public static Result<List<T>> InvertResult<T>(this List<Result<T>> Collection) =>
            Collection.Any(R => R.HasError) ? Collection.First(R => R.HasError).Error : Collection.Select(R => R.Value).ToList().ToResult();

        public static void ForEach<T>(this IEnumerable<T> Collection, Action<T, int> Action)
        {
            int t_Idx = 0;
            Collection.ToList()
                .ForEach(T => Action(T, t_Idx++));
        }

        public static void ForEach<T>(this List<T> Collection, Action<T, int> Action)
        {
            int t_Idx = 0;
            Collection
                .ForEach(T => Action(T, t_Idx++));
        }

        public static IEnumerable<T> SelectConcat<T>(this IEnumerable<T> Collection, Func<T, T> Func) =>
            Collection.Concat(Collection.Select(Func));

        public static IEnumerable<T> SelectManyConcat<T>(this IEnumerable<T> Collection, Func<T, IEnumerable<T>> Func) =>
            Collection.Concat(Collection.SelectMany(Func));

        public static IEnumerable<TSource[]> Chunk<TSource>(this IEnumerable<TSource> Source, int Size)
        {
            var t_ChunkedList = new List<List<TSource>>();
            foreach(var t_Item in Source)
            {
                if(t_ChunkedList.Count == 0 || t_ChunkedList.Last().Count == Size)
                    t_ChunkedList.Add(new List<TSource>());

                t_ChunkedList.Last().Add(t_Item);
            }

            return t_ChunkedList.Select(A => A.ToArray());
        }
    }

}