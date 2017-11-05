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

using System;

namespace Nanomsg2.Sharp
{
    public interface IOptionReader : IOptions
    {
        // TODO: TBD: not sure if we would really need/want to expose the IntPtr based getter

        string GetText(string name);

        string GetText(string name, ref ulong length);

        int GetInt32(string name);

        ulong GetSize(string name);

        double GetTotalMilliseconds(string name);

        TimeSpan GetTimeSpan(string name);

        //// TODO: TBD: add the Address
        //IAddress GetAddress(string name);
    }
}
