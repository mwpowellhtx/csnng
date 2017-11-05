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
using System.Linq;

namespace Nanomsg2.Sharp
{
    internal static class MessagingExtensionMethods
    {
        public static IEnumerable<byte> ToBytes(this string s)
            => s.Select(x => (byte) x);

        public static string ReassembleString(this IEnumerable<byte> bytes)
            => bytes.Aggregate(string.Empty, (g, x) => g + (char) x);
    }
}
