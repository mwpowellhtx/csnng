using System;

namespace Nanomsg2.Sharp
{
    public static class Exceptions
    {
        public delegate bool ExceptionFilter<in T>(T ex)
            where T : Exception;

        public static void Throws<T>(Action action, ExceptionFilter<T> filter)
            where T : Exception
        {
            try
            {
                action();
            }
            catch (T ex) when (filter(ex))
            {
                // Success. Everything else is a failure.
            }
        }
    }
}
