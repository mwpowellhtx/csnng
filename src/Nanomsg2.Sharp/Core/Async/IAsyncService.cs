using System;

namespace Nanomsg2.Sharp
{
    using Messaging;

    public interface IAsyncService : IHaveOne, ICanClose, IDisposable, IHaveOptions<IAsyncOptionWriter>
    {
        void Start();

        void Start(BasicAsyncCallback callback);

        void Wait();

        void TimedWait(int timeoutMilliseconds);

        void TimedWait(TimeSpan timeout);

        void Stop();

        void Cancel();

        void Close(bool force);

        int Result { get; }

        bool Success { get; }

        bool TrySuccess { get; }

        void VerifyResult();

        void Retain(Message message);

        void Cede(Message message);
    }
}
