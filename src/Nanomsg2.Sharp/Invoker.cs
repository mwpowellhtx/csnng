//
// Copyright (c) 2017 Michael W Powell <mwpowellhtx@gmail.com>
// Copyright 2017 Garrett D'Amore <garrett@damore.org>
// Copyright 2017 Capitar IT Group BV <info@capitar.com>
//
// This software is supplied under the terms of the MIT License, a
// copy of which should be located in the distribution where this
// file was obtained (LICENSE.txt).  A copy of the license may also be
// found online at https://opensource.org/licenses/MIT.
//

using System;
using System.Linq;

namespace Nanomsg2.Sharp
{
    using static ErrorCode;

    public class Invoker : Disposable, IInvoker
    {
        protected static InvalidOperationException ThrowInvalidOperation(string operationName)
        {
            return new InvalidOperationException($"{operationName} has not been configured.");
        }

        protected Invoker DefaultInvoker { get; }

        protected Invoker()
        {
            DefaultInvoker = this;
        }

        // TODO: TBD: probably belongs in another class... along the same lines as "invocation" in C++
        protected internal delegate void InvocationHavingNoResult<in T>(T ptr);

        protected internal delegate TResult InvocationWithResultDelegate<out TResult>();

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

        // TODO: TBD: truly, this one may be the better of all the forms: leaving ALL of the impl details to the caller...
        protected internal virtual void InvokeWithDefaultErrorHandling(InvocationWithResultDelegate<int> caller
            , params ErrorCode[] notOneOf)
        {
            var errnum = caller();
            if (!notOneOf.Any())
            {
                notOneOf = new[] {None};
            }
            if (notOneOf.Any(x => errnum == (int) x)) return;
            // TODO: TBD: do something with the result...
            // TODO: TBD: introduce an appropriately named exception
            throw new NanoException(errnum);
        }

        protected internal TResult InvokeWithResult<TResult>(InvocationWithResultDelegate<IntPtr, TResult> caller, IntPtr ptr)
        {
            return caller(ptr);
        }
    }
}
