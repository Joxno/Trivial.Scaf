using System;

namespace Trivial.Functional
{
    public class TryChain<T>
    {
        private Func<T> m_Try = null;
        private Action<Exception> m_Catch = null;
        private Action m_Finally = null;

        public TryChain() { }
        public TryChain(Func<T> Try) => m_Try = Try;
        private TryChain(Func<T> Try, Action<Exception> Catch) => (m_Try, m_Catch) = (Try, Catch);
        private TryChain(Func<T> Try, Action<Exception> Catch, Action Finally) => (m_Try, m_Catch, m_Finally) = (Try, Catch, Finally);

        public TryChain<T> Try(Func<T> Func) => new TryChain<T>(Func);
        public TryChain<T> Catch(Action<Exception> Catch) => 
            new TryChain<T>(m_Try, Catch);
        public TryChain<T> Finally(Action Finally) => 
            new TryChain<T>(m_Try, m_Catch, Finally);

        public Func<Result<T>> ToFunction() =>
            () => 
            { 
                try
                {
                    if (m_Try != null)
                        return m_Try.Invoke();
                    else
                        return new NullReferenceException();
                }
                catch(Exception t_E)
                {
                    m_Catch?.Invoke(t_E);
                    return t_E;
                }
                finally
                {
                    m_Finally?.Invoke();
                }
            };

        public Result<T> Result() =>
            ToFunction()();
    }

    public static class TryChainTest
    {
        public static void Test()
        {
            new TryChain<int>()
                .Try(() => 0)
                .Catch(E => { })
                .Finally(() => { });
        }
    }

}