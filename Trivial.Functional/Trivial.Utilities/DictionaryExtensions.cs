using System.Collections.Concurrent;
using System.Collections.Generic;
using Trivial.Functional;

namespace Trivial.Utilities
{
    public static class DictionaryExtensions
    {
        public static Maybe<TValue> Retrieve<TKey, TValue>(this Dictionary<TKey, TValue> Dictionary, TKey Key) =>
            Dictionary.TryGetValue(Key, out var t_Value)
                ? t_Value
                : null;

        public static Maybe<TValue> Retrieve<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> Dictionary, TKey Key) =>
            Dictionary.TryGetValue(Key, out var t_Value)
                ? t_Value
                : null;
    }
}