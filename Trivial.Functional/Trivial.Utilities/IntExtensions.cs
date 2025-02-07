using System;
using Trivial.Functional;

namespace Trivial.Utilities
{
    public static class IntExtensions
    {
        public static Result<Unit> ForEach(this int I, Action<int> A)
        {
            I = Math.Abs(I);
            for (var t_I = 0; t_I < I; t_I++)
                A(t_I);
            return Defaults.Unit;
        }

        public static int Clamp(this int I, int Min, int Max, out int Remainder)
        {
            var t_Value = Math.Min(Math.Max(I, Min), Max);
            Remainder = I - t_Value;
            return t_Value;
        }
    }
}