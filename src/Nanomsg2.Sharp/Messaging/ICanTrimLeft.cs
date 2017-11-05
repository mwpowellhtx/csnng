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

namespace Nanomsg2.Sharp.Messaging
{
    public interface ICanTrimLeft
    {
    }

    public interface ICanTrimLeft<TResult> : ICanTrimLeft
    {
        void TrimLeft(out TResult result);
    }

    // TODO: TBD: consider consolidating trim left/right: need parameters such as count or sz, and the out value
    // TODO: TBD: this sounds like a decent comprehension of the C API, but save for just after establishing VerCtrl baseline
    public interface ICanTrimLeft<in TCount, TResult> : ICanTrimLeft
    {
        void TrimLeft(TCount count, out TResult result);
    }
}
