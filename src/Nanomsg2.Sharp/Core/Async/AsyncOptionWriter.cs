using System;

namespace Nanomsg2.Sharp
{
    internal delegate void SetTimeoutDurationMillisecondsDelegate(int value);

    public class AsyncOptionWriter : Invoker, IAsyncOptionWriter
    {
        private SetTimeoutDurationMillisecondsDelegate _setter;

        internal virtual void SetSetters(SetTimeoutDurationMillisecondsDelegate setter)
        {
            // We have but the one lonely Setter in this case.
            _setter = setter;
        }

        private readonly IAsyncService _svc;

        internal AsyncOptionWriter(IAsyncService svc)
        {
            _svc = svc;
        }

        public bool HasOne => !ReferenceEquals(null, _svc);

        public virtual void SetTimeoutDurationMilliseconds(int value)
        {
            InvokeHavingNoResult(() => _setter(value));
        }

        public virtual void SetTimeoutDuration(TimeSpan value)
        {
            SetTimeoutDurationMilliseconds((int)value.TotalMilliseconds);
        }
    }
}
