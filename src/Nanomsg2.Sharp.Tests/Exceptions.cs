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
using System.Linq.Expressions;

namespace Nanomsg2.Sharp
{
    using Xunit;

    public static class Exceptions
    {
        public delegate bool ExceptionFilter<in T>(T ex)
            where T : Exception;

        public static void Matching<T>(this T ex, Expression<ExceptionFilter<T>> filterExpr)
            where T : Exception
        {
            var filter = filterExpr.Compile();
            Assert.True(filter(ex), $"Expected exception {typeof(T)} to be thrown but was not. Using filter: {filterExpr.Body}");
        }

        [Obsolete("The xUnit Assert.Throws returns the exception, so use the Matching")]
        public static void Throws<T>(Action action, Expression<ExceptionFilter<T>> filterExpr)
            where T : Exception
        {
            var filter = filterExpr.Compile();
            try
            {
                action();
                Assert.True(false, $"Expected exception {typeof(T)} to be thrown but was not.");
            }
            catch (T ex) when (filter(ex))
            {
                // Success. Everything else is a failure.
            }
            catch
            {
                // It's not perfect, but it will give an indication what's going on.
                Assert.True(false, $"Expected exception {typeof(T)} to be thrown but was not. Using filter: {filterExpr.Body}");
            }
        }
    }
}
