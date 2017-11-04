using System;

namespace Nanomsg2.Sharp
{
    public interface IEndPoint : IHaveOne, ICanClose, IHaveOptions<IOptionReaderWriter>, IDisposable
    {
        void Start(SocketFlag flags);
    }
}
