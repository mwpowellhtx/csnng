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
            , ICanTrimLeft<uint>
            , ICanTrimLeft<int, IEnumerable<uint>>
            , ICanTrimLeft<ulong, IEnumerable<byte>>
            , ICanTrimLeft<int, string>
            , ICanTrimRight<uint>
            , ICanTrimRight<int, IEnumerable<uint>>
            , ICanTrimRight<ulong, IEnumerable<byte>>
            , ICanTrimRight<int, string>
            , ICanGet<IEnumerable<byte>>
    {
        IMessage Parent { get; }
    }
}
