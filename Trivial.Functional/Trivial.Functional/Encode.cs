using System;

namespace Trivial.Functional
{
    public static partial class Functions
    {
        public static Func<T> EncodeState<T>(Func<Func<T>> F) => F();
    }

}