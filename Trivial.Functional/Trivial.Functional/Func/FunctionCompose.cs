using System;

namespace Trivial.Functional
{
    public static partial class Function
    {
        public static Func<T, TR> Compose<T, T2, TR>(Func<T2, TR> A, Func<T, T2> B) =>
            P => A(B(P));
    }
}