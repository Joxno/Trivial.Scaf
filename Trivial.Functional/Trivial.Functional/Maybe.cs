using System;

namespace Trivial.Functional
{
    public struct Maybe<T>
    {
        private TypeWrapper<T> m_Value;

        public Maybe(T Value) => 
            m_Value = 
            Value != null ? 
            new TypeWrapper<T> { WrappedValue = Value } : 
            null;

        public bool HasValue => m_Value != null;

        public T Value => !HasValue ? throw new NullReferenceException() : m_Value.WrappedValue;

        public static implicit operator Maybe<T>(T Value) => Value != null ? new Maybe<T>(Value) : new Maybe<T>();
        public static implicit operator Maybe<T>(NoValue NoVal) => new Maybe<T>();
        //public static implicit operator bool(Maybe<T> M) => M.HasValue;
        public static bool operator ==(Maybe<T> Left, T Right) =>
            !Left.HasValue ? false : Left.Value.Equals(Right);
        public static bool operator !=(Maybe<T> Left, T Right) =>
            !(Left == Right);
        public static bool operator ==(T Left, Maybe<T> Right) =>
            Right == Left;
        public static bool operator !=(T Left, Maybe<T> Right) =>
            Right != Left;
        public static bool operator true(Maybe<T> M) => M.HasValue;
        public static bool operator false(Maybe<T> M) => !M.HasValue;
        public static Maybe<T> operator |(Maybe<T> M, Func<T, T> Func) =>
            M ? Func(M.Value) : M;
        public static Maybe<T> operator |(Maybe<T> M, Action<T> TapFunc) =>
            M ? M.Value.Tap(TapFunc) : M;

        /* Container for wrapping types 
        * in order to support class, struct and interface types in Maybe */
        private class TypeWrapper<TYpe>
        {
            public TYpe WrappedValue { get; set; }

            public static implicit operator TypeWrapper<TYpe>(TYpe Value) =>
                new TypeWrapper<TYpe> { WrappedValue = Value };
        }

        /* >>= */
        public static Maybe<T2> Bind<T2>(Maybe<T> Value, Func<T, Maybe<T2>> Func) =>
            Value.HasValue ? Func(Value.m_Value.WrappedValue) : null;

        /* >> */
        public static Maybe<T2> Bind<T2>(Maybe<T> Value, Func<Maybe<T2>> Func) =>
            Bind(Value, Function.Pipe(Functions.Identity<T>(), () => Func()));

        /* >>= */
        public Maybe<T2> Bind<T2>(Func<T, Maybe<T2>> Func) =>
            Bind(this, Func);

        /* >> */
        public Maybe<T2> Bind<T2>(Func<Maybe<T2>> Func) =>
            Bind(this, Func);

        /* Select */
        public Maybe<T2> Map<T2>(Func<T, T2> Func) =>
            Bind(this, (P) => new Maybe<T2>(Func(P)));

        internal Type[] ToArray()
        {
            throw new NotImplementedException();
        }
    }

    public class NoValue { }

    public static class Maybe
    {
        public static Maybe<T> Some<T>(T Value) => Value;
        public static NoValue Null => new NoValue();
        public static NoValue None => Null;
        public static Func<Maybe<T>> Lift<T>(Func<T> Func) => () => Func();
        public static Maybe<T> Return<T>(T Value) => Value;

    }

}