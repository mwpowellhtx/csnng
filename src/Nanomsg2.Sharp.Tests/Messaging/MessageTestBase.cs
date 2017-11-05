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
using System.Linq;

namespace Nanomsg2.Sharp.Messaging
{
    using Xunit;

    public abstract class MessageTestBase
    {
        // ReSharper disable once InconsistentNaming
        protected const ulong sizeof_uint = sizeof(uint);

        // ReSharper disable once InconsistentNaming
        protected const string this_is_a_test = "this is a test";

        // ReSharper disable once InconsistentNaming
        protected const string this_is_your_life = "this is your life";

        // ReSharper disable once InconsistentNaming
        protected static readonly byte[] sample_data;

        static MessageTestBase()
        {
            sample_data = Enumerable.Range(0, 16).Select(x => (byte) x).ToArray();
        }

        protected static void VerifyMessage(Action<IMessage> action)
            => VerifyMessage(default(ulong), action);

        protected static void VerifyMessage(ulong sz, Action<IMessage> action)
        {
            Assert.NotNull(action);
            using (var m = new Message(sz))
            {
                action(m);
            }
        }

        protected static void VerifyHavingOne(IHaveOne obj, bool expected)
        {
            Assert.NotNull(obj);
            Assert.Equal(expected, obj.HasOne);
        }

        protected static void VerifySize(IHaveSize obj, ulong expected)
        {
            Assert.NotNull(obj);
            Assert.Equal(expected, obj.Size);
        }
    }
}
