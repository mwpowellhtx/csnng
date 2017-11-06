using System;

namespace Nanomsg2.Sharp
{
    using Messaging;

    public interface IAsyncService : IHaveOne, ICanClose, IDisposable
    {
        void Start();

        void Start(BasicAsyncCallback callback);

        void Wait();

        void Stop();

        void Cancel();

        void Close(bool force);

        int Result { get; }

        bool Success { get; }

        bool TrySuccess { get; }

        void VerifyResult();

        void SetTimeoutDurationMilliseconds(int value);

        void SetTimeoutDuration(TimeSpan value);

        void Retain(Message message);

        void Cede(Message message);
    }

    public static class AsyncServiceExtensionMethods
    {
        public static void SetTimeout<T>(this T svc, Duration value)
            where T : class, IAsyncService
        {
            svc.SetTimeoutDurationMilliseconds(value.ToInt());
        }
    }
}
