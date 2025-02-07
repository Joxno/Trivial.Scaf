using System;
using Trivial.Functional;
using Trivial.Utilities;

namespace Trivial.Utilities;

public static class TypeExtensions
{
    public static void Is<T>(this object Object, Action<T> IfIs)
    {
        if(Object is T t_AsT)
            IfIs(t_AsT);
    }

    public static Maybe<T> Is<T>(this object Object) =>
        Object is T t_AsT ? t_AsT : default;

    public static bool IsNull<T>(this T Object) =>
        Object is null;

    public static T As<T>(this object Object) where T : class =>
        Object as T;
    public static T As<T>(this object Object, Action<T> OnNotNull) where T : class =>
        (Object as T).Tap(O => O.ToMaybe().Then(Mo => OnNotNull?.Invoke(Mo)));
    public static Maybe<T2> As<T1, T2>(this Maybe<T1> Object) where T1 : class where T2 : class
    {
        if(Object.HasValue)
            return Object.Value as T2;
        return default;
    }

    public static Maybe<T2> As<T1, T2>(this Maybe<T1> Object, Action<T2> OnNotNull) where T1 : class where T2 : class
    {
        if(Object.HasValue)
            return (Object.Value as T2).Tap(OnNotNull);
        return default;
    }

    public static T On<T>(this T Obj, T Val, Action OnEquals) =>
        Obj.Tap(O => { if(Obj.Equals(Val)) OnEquals(); });

    public static void AsAny<T1, T2>(this object Object, Action<T1> AsT1, Action<T2> AsT2)
    {
        if(Object is T1 t_T1Obj)
            AsT1(t_T1Obj);
        else if(Object is T2 t_T2Obj)
            AsT2(t_T2Obj);
    }

    public static void AsAny<T1, T2, T3>(this object Object, Action<T1> AsT1, Action<T2> AsT2, Action<T3> AsT3)
    {
        if(Object is T1 t_T1Obj)
            AsT1(t_T1Obj);
        else if(Object is T2 t_T2Obj)
            AsT2(t_T2Obj);
        else if(Object is T3 t_T3Obj)
            AsT3(t_T3Obj);
    }

    public static void AsAny<T1, T2, T3, T4>(this object Object, Action<T1> AsT1, Action<T2> AsT2, Action<T3> AsT3, Action<T4> AsT4)
    {
        if(Object is T1 t_T1Obj)
            AsT1(t_T1Obj);
        else if(Object is T2 t_T2Obj)
            AsT2(t_T2Obj);
        else if(Object is T3 t_T3Obj)
            AsT3(t_T3Obj);
        else if(Object is T4 t_T4Obj)
            AsT4(t_T4Obj);
    }
}