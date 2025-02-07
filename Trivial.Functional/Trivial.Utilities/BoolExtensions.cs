using System;
using Trivial.Functional;

namespace Trivial.Utilities
{
    public static class BoolExtensions
    {
        public static bool IfTrue(this bool B, Action A) => B.Tap(B => { if (B) A(); });
        public static bool IfFalse(this bool B, Action A) => B.Tap(B => { if (!B) A(); });
        public static bool IfElse(this bool B, Action OnTrue, Action OnFalse) => B.Tap(B => { if (B) OnTrue(); else OnFalse(); });
    }
}