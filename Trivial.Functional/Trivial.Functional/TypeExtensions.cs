using System;

namespace Trivial.Functional
{
    public static class TypeExtensions
    {
        public static object EraseType<T>(this T Value) => Value;

        public static (object, Type) StripType<T>(this T Value) => (Value, typeof(T));
    }
}