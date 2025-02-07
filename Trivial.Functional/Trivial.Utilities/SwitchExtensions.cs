using System;

namespace Trivial.Utilities
{
    public static class SwitchExtensions
    {
        public static void Switch<TObject, T1>(this TObject O, Action<T1> M1, Action Else = null)
        {
            if (O is T1 t_P1)
                M1?.Invoke(t_P1);
            else
                Else?.Invoke();
        }

        public static void Switch<TObject, T1, T2>(this TObject O, Action<T1> M1, Action<T2> M2, Action Else = null)
        {
            if (O is T1 t_P1)
                M1?.Invoke(t_P1);
            else if (O is T2 t_P2)
                M2?.Invoke(t_P2);
            else
                Else?.Invoke();
        }

        public static void Switch<TObject, T1, T2, T3>(this TObject O, Action<T1> M1, Action<T2> M2, Action<T3> M3, Action Else = null)
        {
            if (O is T1 t_P1)
                M1?.Invoke(t_P1);
            else if (O is T2 t_P2)
                M2?.Invoke(t_P2);
            else if (O is T3 t_P3)
                M3?.Invoke(t_P3);
            else
                Else?.Invoke();
        }

        public static void Switch<TObject, T1, T2, T3, T4>(this TObject O, Action<T1> M1, Action<T2> M2, Action<T3> M3, Action<T4> M4, Action Else = null)
        {
            if (O is T1 t_P1)
                M1?.Invoke(t_P1);
            else if (O is T2 t_P2)
                M2?.Invoke(t_P2);
            else if (O is T3 t_P3)
                M3?.Invoke(t_P3);
            else if (O is T4 t_P4)
                M4?.Invoke(t_P4);
            else
                Else?.Invoke();
        }

        public static void Switch<TObject, T1, T2, T3, T4, T5>(this TObject O, Action<T1> M1, Action<T2> M2, Action<T3> M3, Action<T4> M4, Action<T5> M5, Action Else = null)
        {
            if (O is T1 t_P1)
                M1?.Invoke(t_P1);
            else if (O is T2 t_P2)
                M2?.Invoke(t_P2);
            else if (O is T3 t_P3)
                M3?.Invoke(t_P3);
            else if (O is T4 t_P4)
                M4?.Invoke(t_P4);
            else if (O is T5 t_P5)
                M5?.Invoke(t_P5);
            else
                Else?.Invoke();
        }

        public static void Switch<TObject, T1, T2, T3, T4, T5, T6>(this TObject O, Action<T1> M1, Action<T2> M2, Action<T3> M3, Action<T4> M4, Action<T5> M5, Action<T6> M6, Action Else = null)
        {
            if (O is T1 t_P1)
                M1?.Invoke(t_P1);
            else if (O is T2 t_P2)
                M2?.Invoke(t_P2);
            else if (O is T3 t_P3)
                M3?.Invoke(t_P3);
            else if (O is T4 t_P4)
                M4?.Invoke(t_P4);
            else if (O is T5 t_P5)
                M5?.Invoke(t_P5);
            else if (O is T6 t_P6)
                M6?.Invoke(t_P6);
            else
                Else?.Invoke();
        }

        public static void Switch<TObject, T1, T2, T3, T4, T5, T6, T7>(this TObject O, Action<T1> M1, Action<T2> M2, Action<T3> M3, Action<T4> M4, Action<T5> M5, Action<T6> M6, Action<T7> M7, Action Else = null)
        {
            if (O is T1 t_P1)
                M1?.Invoke(t_P1);
            else if (O is T2 t_P2)
                M2?.Invoke(t_P2);
            else if (O is T3 t_P3)
                M3?.Invoke(t_P3);
            else if (O is T4 t_P4)
                M4?.Invoke(t_P4);
            else if (O is T5 t_P5)
                M5?.Invoke(t_P5);
            else if (O is T6 t_P6)
                M6?.Invoke(t_P6);
            else if (O is T7 t_P7)
                M7?.Invoke(t_P7);
            else
                Else?.Invoke();
        }

    }
}