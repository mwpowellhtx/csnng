//
// Copyright (c) 2017 Michael W Powell <mwpowellhtx@gmail.com>
// Copyright 2017 Garrett D'Amore <garrett@damore.org>
// Copyright 2017 Capitar IT Group BV <info@capitar.com>
//
// This software is supplied under the terms of the MIT License, a
// copy of which should be located in the distribution where this
// file was obtained (LICENSE.txt).  A copy of the license may also be
// found online at https://opensource.org/licenses/MIT.
//

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
