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
    public interface ICanPrepend
    {
    }

    public interface ICanPrepend<in T> : ICanPrepend
    {
        void Prepend(T value);
    }


    public interface ICanPrependWithSize<in T> : ICanAppend
    {
        void Prepend(T value, ulong sz);
    }
}
