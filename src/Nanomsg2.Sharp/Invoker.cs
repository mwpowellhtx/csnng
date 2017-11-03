using System;

namespace Nanomsg2.Sharp
{
    public class Invoker : Disposable, IInvoker
    {
        // TODO: TBD: probably belongs in another class... along the same lines as "invocation" in C++
        protected internal delegate void InvocationHavingNoResult<in T>(T ptr);

        protected internal delegate TResult InvocationWithResultDelegate<in T, out TResult>(T ptr);

        protected internal virtual void InvokeHavingNoResult<T>(InvocationHavingNoResult<T> caller, T ptr)
        {
            caller(ptr);
        }

        protected internal virtual void InvokeWithDefaultErrorHandling<T>(InvocationWithResultDelegate<T, int> caller, T ptr)
        {
            var result = caller(ptr);
        }

        protected internal virtual TResult InvokeWithResult<T, TResult>(InvocationWithResultDelegate<T, TResult> caller, T ptr)
        {
            var result = caller(ptr);
            return result;
        }

        // TODO: TBD: will need to consider the "throw if one of" scenario...
        protected internal virtual void InvokeWithDefaultErrorHandling(InvocationWithResultDelegate<IntPtr, int> caller, IntPtr ptr)
        {
            var errnum = caller(ptr);
            if (errnum == 0) return;
            // TODO: TBD: do something with the result...
            // TODO: TBD: introduce an appropriately named exception
            throw new NanoException(errnum);
        }

        protected internal virtual TResult InvokeWithResult<TResult>(InvocationWithResultDelegate<IntPtr, TResult> caller, IntPtr ptr)
        {
            return caller(ptr);
        }
    }
}
