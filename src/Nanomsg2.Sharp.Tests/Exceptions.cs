using System;
using System.Linq.Expressions;

namespace Nanomsg2.Sharp
{
    using Xunit;

    public static class Exceptions
    {
        public delegate bool ExceptionFilter<in T>(T ex)
            where T : Exception;

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
