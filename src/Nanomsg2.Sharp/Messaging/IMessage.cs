using System;

namespace Nanomsg2.Sharp.Messaging
{
    public interface IMessage : IHaveOne, IHaveSize, ICanClear, IDisposable
    {
        IHeaderPart Header { get; }

        IBodyPart Body { get; }
    }
}
