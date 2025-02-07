using System;

namespace Trivial.Functional
{
    public static partial class Functions
    {
        public static Func<int> CreateIncrementer(int Start = 0) =>
            EncodeState<int>(() =>
            {
                var t_Id = Start;
                return () => t_Id++;
            });

        public static Func<int> CreateDecrementer(int Start = 0) =>
            EncodeState<int>(() =>
            {
                var t_Id = Start;
                return () => t_Id--;
            });

        public static Func<T> EncodeCallback<T>(Func<T> F, Action Callback) =>
            EncodeState<T>(() =>
            {
                var t_Func = F;
                var t_Callback = Callback;

                return () => t_Func().Tap(_ => t_Callback());
            });
    }
}
