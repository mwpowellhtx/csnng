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
using System.Text;

namespace Nanomsg2.Sharp
{
    // TODO: TBD: not sure we would need the raw IntPtr based getter; most times that is supporting String, that I know of, anyhow...
    internal delegate int GetOptDelegate(string name, ref IntPtr valuePtr, ref ulong sz);

    internal delegate int GetOptStringBuilderDelegate(string name, StringBuilder valuePtr, ref ulong length);

    internal delegate int GetOptDelegate<T>(string name, ref T value);

    internal delegate int SetOptDelegate(string name, ref IntPtr valuePtr, ulong sz);

    internal delegate int SetOptStringDelegate(string name, string s, ulong sz);

    internal delegate int SetOptDelegate<in T>(string name, T value);
}
