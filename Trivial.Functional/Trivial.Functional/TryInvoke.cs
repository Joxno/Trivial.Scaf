using System;

namespace Trivial.Functional
{
    public class TryInvoke<T>
    {
        private Func<Result<T>> m_Execution;

        public TryInvoke(Func<T> Func) => m_Execution = Try.GetInvoke(Func);
        private TryInvoke(Func<Result<T>> Func) => m_Execution = Func;

        public static TryInvoke<T2> Bind<T2>(TryInvoke<T> M, Func<T, TryInvoke<T2>> Func) =>
            Function.Pipe(
                M.m_Execution, 
                (Result<T> P) => !P.HasError ? Func(P.Value).m_Execution() : (Result<T2>)P.Error);

        public static TryInvoke<T2> Bind<T2>(TryInvoke<T> M, Func<TryInvoke<T2>> Func) =>
            Bind(M, Function.Pipe(Functions.Identity<T>(), () => Func()));

        public TryInvoke<T2> Bind<T2>(Func<T, TryInvoke<T2>> Func) =>
            Bind(this, Func);

        public TryInvoke<T2> Bind<T2>(Func<TryInvoke<T2>> Func) =>
            Bind(this, Func);

        public TryInvoke<T2> Map<T2>(Func<T, T2> Func) =>
            Bind(this, (P) => new TryInvoke<T2>(() => Func(P)));

        public Result<T> Run() => m_Execution();

        public static implicit operator TryInvoke<T>(Func<Result<T>> Func) => new TryInvoke<T>(Func);
    }
}