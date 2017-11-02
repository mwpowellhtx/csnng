using System.Collections.Generic;

namespace Nanomsg2.Sharp.Messaging
{
    public interface IMessagePart
        : IHaveOne, IHaveSize
            , ICanClear
            , ICanAppend<uint>
            , ICanAppend<string>
            , ICanAppend<IEnumerable<byte>>
            , ICanAppendWithSize<IEnumerable<byte>>
            , ICanPrepend<uint>
            , ICanPrepend<string>
            , ICanPrepend<IEnumerable<byte>>
            , ICanPrependWithSize<IEnumerable<byte>>
            , ICanTrimLeft<uint>, ICanTrimRight<uint>
            , ICanTrimBytesLeft<ulong>, ICanTrimBytesRight<ulong>
            , ICanGet<byte[]>
    {
        IMessage Parent { get; }
    }
}
