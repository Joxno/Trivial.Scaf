namespace Trivial.Utilities;

public static class SwitchValueExtensions
{
    public static void SwitchOnValue<TObject, T1>(this TObject O, (T1 V, Action A) M1, Action Else = null)
    {
        if(O.Equals(M1.V)) M1.A?.Invoke();
        else Else?.Invoke();
    }
    
    public static void SwitchOnValue<TObject, T1, T2>(this TObject O, (T1 V, Action A) M1, (T2 V, Action A) M2, Action Else = null)
    {
        if(O.Equals(M1.V)) M1.A?.Invoke();
        else if(O.Equals(M2.V)) M2.A?.Invoke();
        else Else?.Invoke();
    }
    
    public static void SwitchOnValue<TObject, T1, T2, T3>(this TObject O, 
        (T1 V, Action A) M1, 
        (T2 V, Action A) M2,
        (T3 V, Action A) M3, 
        Action Else = null)
    {
        if(O.Equals(M1.V)) M1.A?.Invoke();
        else if(O.Equals(M2.V)) M2.A?.Invoke();
        else if(O.Equals(M3.V)) M3.A?.Invoke();
        else Else?.Invoke();
    }

    public static void SwitchOnValue<TObject, T1, T2, T3, T4>(this TObject O, 
        (T1 V, Action A) M1, 
        (T2 V, Action A) M2,
        (T3 V, Action A) M3, 
        (T4 V, Action A) M4, 
        Action Else = null)
    {
        if(O.Equals(M1.V)) M1.A?.Invoke();
        else if(O.Equals(M2.V)) M2.A?.Invoke();
        else if(O.Equals(M3.V)) M3.A?.Invoke();
        else if(O.Equals(M4.V)) M4.A?.Invoke();
        else Else?.Invoke();
    }

    public static void SwitchOnValue<TObject, T1, T2, T3, T4, T5>(this TObject O, 
        (T1 V, Action A) M1, 
        (T2 V, Action A) M2,
        (T3 V, Action A) M3, 
        (T4 V, Action A) M4, 
        (T5 V, Action A) M5, 
        Action Else = null)
    {
        if(O.Equals(M1.V)) M1.A?.Invoke();
        else if(O.Equals(M2.V)) M2.A?.Invoke();
        else if(O.Equals(M3.V)) M3.A?.Invoke();
        else if(O.Equals(M4.V)) M4.A?.Invoke();
        else if(O.Equals(M5.V)) M5.A?.Invoke();
        else Else?.Invoke();
    }

    public static void SwitchOnValue<TObject, T1, T2, T3, T4, T5, T6>(this TObject O, 
        (T1 V, Action A) M1, 
        (T2 V, Action A) M2,
        (T3 V, Action A) M3, 
        (T4 V, Action A) M4, 
        (T5 V, Action A) M5, 
        (T6 V, Action A) M6, 
        Action Else = null)
    {
        if(O.Equals(M1.V)) M1.A?.Invoke();
        else if(O.Equals(M2.V)) M2.A?.Invoke();
        else if(O.Equals(M3.V)) M3.A?.Invoke();
        else if(O.Equals(M4.V)) M4.A?.Invoke();
        else if(O.Equals(M5.V)) M5.A?.Invoke();
        else if(O.Equals(M6.V)) M6.A?.Invoke();
        else Else?.Invoke();
    }

    public static TReturn SwitchOnValue<TObject, T1, TReturn>(this TObject O, (T1 V, Func<TReturn> A) M1, Func<TReturn> Else = null)
    {
        if(O.Equals(M1.V) && M1.A is not null) return M1.A.Invoke();
        else if(Else is not null) return Else.Invoke();

        return default;
    }

    public static TReturn SwitchOnValue<TObject, T1, T2, TReturn>(this TObject O, (T1 V, Func<TReturn> A) M1, (T2 V, Func<TReturn> A) M2, Func<TReturn> Else = null)
    {
        if(O.Equals(M1.V) && M1.A is not null) return M1.A.Invoke();
        else if(O.Equals(M2.V) && M2.A is not null) return M2.A.Invoke();
        else if(Else is not null) return Else.Invoke();

        return default;
    }

    public static TReturn SwitchOnValue<TObject, T1, T2, T3, TReturn>(this TObject O, 
        (T1 V, Func<TReturn> A) M1, 
        (T2 V, Func<TReturn> A) M2,
        (T3 V, Func<TReturn> A) M3, 
        Func<TReturn> Else = null)
    {
        if(O.Equals(M1.V) && M1.A is not null) return M1.A.Invoke();
        else if(O.Equals(M2.V) && M2.A is not null) return M2.A.Invoke();
        else if(O.Equals(M3.V) && M3.A is not null) return M3.A.Invoke();
        else if(Else is not null) return Else.Invoke();

        return default;
    }

    public static TReturn SwitchOnValue<TObject, T1, T2, T3, T4, TReturn>(this TObject O, 
        (T1 V, Func<TReturn> A) M1, 
        (T2 V, Func<TReturn> A) M2,
        (T3 V, Func<TReturn> A) M3, 
        (T4 V, Func<TReturn> A) M4, 
        Func<TReturn> Else = null)
    {
        if(O.Equals(M1.V) && M1.A is not null) return M1.A.Invoke();
        else if(O.Equals(M2.V) && M2.A is not null) return M2.A.Invoke();
        else if(O.Equals(M3.V) && M3.A is not null) return M3.A.Invoke();
        else if(O.Equals(M4.V) && M4.A is not null) return M4.A.Invoke();
        else if(Else is not null) return Else.Invoke();

        return default;
    }

    public static TReturn SwitchOnValue<TObject, T1, T2, T3, T4, T5, TReturn>(this TObject O, 
        (T1 V, Func<TReturn> A) M1, 
        (T2 V, Func<TReturn> A) M2,
        (T3 V, Func<TReturn> A) M3, 
        (T4 V, Func<TReturn> A) M4, 
        (T5 V, Func<TReturn> A) M5, 
        Func<TReturn> Else = null)
    {
        if(O.Equals(M1.V) && M1.A is not null) return M1.A.Invoke();
        else if(O.Equals(M2.V) && M2.A is not null) return M2.A.Invoke();
        else if(O.Equals(M3.V) && M3.A is not null) return M3.A.Invoke();
        else if(O.Equals(M4.V) && M4.A is not null) return M4.A.Invoke();
        else if(O.Equals(M5.V) && M5.A is not null) return M5.A.Invoke();
        else if(Else is not null) return Else.Invoke();

        return default;
    }

    public static TReturn SwitchOnValue<TObject, T1, T2, T3, T4, T5, T6, TReturn>(this TObject O, 
        (T1 V, Func<TReturn> A) M1, 
        (T2 V, Func<TReturn> A) M2,
        (T3 V, Func<TReturn> A) M3, 
        (T4 V, Func<TReturn> A) M4, 
        (T5 V, Func<TReturn> A) M5, 
        (T6 V, Func<TReturn> A) M6, 
        Func<TReturn> Else = null)
    {
        if(O.Equals(M1.V) && M1.A is not null) return M1.A.Invoke();
        else if(O.Equals(M2.V) && M2.A is not null) return M2.A.Invoke();
        else if(O.Equals(M3.V) && M3.A is not null) return M3.A.Invoke();
        else if(O.Equals(M4.V) && M4.A is not null) return M4.A.Invoke();
        else if(O.Equals(M5.V) && M5.A is not null) return M5.A.Invoke();
        else if(O.Equals(M6.V) && M6.A is not null) return M6.A.Invoke();
        else if(Else is not null) return Else.Invoke();

        return default;
    }
}