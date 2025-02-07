using System;

namespace Trivial.Functional
{
    public static class Try
    {
        public static Maybe<T> From<T>(Func<T> Func)
        {
            try
            {
                return new Maybe<T>(Func());
            }
            catch
            {
                return new Maybe<T>();
            }
        }

        public static Func<T, Maybe<T2>> GetFrom<T, T2>(Func<T, T2> Func) =>
            (P) =>
            {
                try
                {
                    return new Maybe<T2>(Func(P));
                }
                catch
                {
                    return new Maybe<T2>();
                }
            };

        public static Func<T, Result<Maybe<T2>>> GetInvokeFrom<T, T2>(Func<T, T2> Func) =>
            (P) =>
            {
                try
                {
                    return new Result<Maybe<T2>>(new Maybe<T2>(Func(P)));
                }
                catch(Exception t_E)
                {
                    return new Result<Maybe<T2>>(t_E);
                }
            };

        public static Result<T> Invoke<T>(Func<T> Func)
        {
            try
            {
                return new Result<T>(Func());
            }
            catch(Exception t_E)
            {
                return new Result<T>(t_E);
            }
        }

        public static async Task<Result<T>> InvokeAsync<T>(Func<Task<T>> Func)
        {
            try
            {
                return new Result<T>(await Func());
            }
            catch(Exception t_E)
            {
                return new Result<T>(t_E);
            }
        }

        public static Result<T2> Invoke<T, T2>(Func<T, T2> Func, T Val)
        {
            try
            {
                return new Result<T2>(Func(Val));
            }
            catch (Exception t_E)
            {
                return new Result<T2>(t_E);
            }
        }

        public static Func<T, Result<T2>> GetInvoke<T, T2>(Func<T, T2> Func) =>
            (P) =>
            {
                try
                {
                    return new Result<T2>(Func(P));
                }
                catch (Exception t_E)
                {
                    return new Result<T2>(t_E);
                }
            };

        public static Func<Result<T>> GetInvoke<T>(Func<T> Func) =>
            () =>
            {
                try
                {
                    return new Result<T>(Func());
                }
                catch (Exception t_E)
                {
                    return new Result<T>(t_E);
                }
            };

        public static Result<Unit> Invoke(Action Method)
        {
            try
            {
                Method?.Invoke();
                return new Result<Unit>(Defaults.Unit);
            }
            catch (Exception t_E)
            {
                return new Result<Unit>(t_E);
            }
        }

        public static Result<Unit> Invoke<T>(Action<T> Method, T Val)
        {
            try
            {
                Method?.Invoke(Val);
                return new Result<Unit>(Defaults.Unit);
            }
            catch (Exception t_E)
            {
                return new Result<Unit>(t_E);
            }
        }
    }
}