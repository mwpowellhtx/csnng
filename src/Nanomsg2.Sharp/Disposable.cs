using System;

namespace Nanomsg2.Sharp
{
    public class Disposable : IDisposable
    {
        ~Disposable()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        protected bool IsDisposed { get; private set; }

        public void Dispose()
        {
            Dispose(true);
            IsDisposed = true;
        }
    }
}
