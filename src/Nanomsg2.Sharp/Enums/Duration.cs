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

namespace Nanomsg2.Sharp
{
    public enum Duration : int
    {
        Default = -2,
        Infinite,
        Zero
    }

    public static class DurationExtensionMethods
    {
        public static int ToInt(this Duration value) => (int) value;

        public static Duration ToDuration(this int value) => (Duration) value;

        // TODO: TBD: perhaps also conversions to TimeSpan?
    }
}
