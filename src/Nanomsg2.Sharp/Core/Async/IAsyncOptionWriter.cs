using System;

namespace Nanomsg2.Sharp
{
    public interface IAsyncOptionWriter : IOptions
    {
        void SetTimeoutDurationMilliseconds(int value);

        void SetTimeoutDuration(TimeSpan value);
    }

    public static class AsyncOptionWriterExtensionMethods
    {
        public static void SetTimeout<T>(this T opt, Duration value)
            where T : class, IAsyncOptionWriter
        {
            opt.SetTimeoutDurationMilliseconds(value.ToInt());
        }
    }
}
