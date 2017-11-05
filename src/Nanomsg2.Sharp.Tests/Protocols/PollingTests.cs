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
using System.Runtime.InteropServices;
using System.Threading;

namespace Nanomsg2.Sharp.Protocols
{
    using Pair;
    using Xunit;
    using Xunit.Abstractions;
    using static PollEvent;
    using static Thread;
    using static TimeSpan;
    using static Imports;
    using O = Options;

    public class PollingTests : BehaviorDrivenTestFixtureBase
    {
        [DllImport(Ws232Dll, EntryPoint = "WSAPoll", CallingConvention = StdCall)]
        private static extern int WsaPoll(ref POLLFD value, ulong fds, int timeout);

        private const int InvalidFd = -1;

        private const string Addr = "inproc://yeahbaby";

        private const string Kick = "kick";

        // ReSharper disable once InconsistentNaming
        private ScenarioDelegate Given_connected_Pair_Sockets { get; }

        private Socket[] _sockets;

        public PollingTests(ITestOutputHelper @out)
            : base(@out)
        {
            Given_connected_Pair_Sockets = action =>
            {
                Section("Given a connected Pair of Sockets", () =>
                {
                    _sockets = new Socket[]
                    {
                        new LatestPairSocket(),
                        new LatestPairSocket()
                    };

                    var s1 = _sockets[0];
                    var s2 = _sockets[1];

                    var timeout = FromMilliseconds(50d);

                    s1.Listen(Addr);
                    Sleep(timeout);
                    s2.Dial(Addr);
                    Sleep(timeout);

                    action();

                    _sockets?[0]?.Dispose();
                    _sockets?[1]?.Dispose();
                });
            };
        }

        private void We_can_get_a_Recv_FD(Action<int> action)
        {
            Given_connected_Pair_Sockets(() =>
            {
                var s1 = _sockets[0];

                Section("We can get a Recv FD", () =>
                {
                    var fd = s1.Options.GetInt32(O.RecvFd);
                    Assert.NotEqual(InvalidFd, fd);

                    action(fd);
                });
            });
        }

        [Fact]
        public void And_is_always_the_same_FD()
        {
            We_can_get_a_Recv_FD(fd =>
            {
                Section("is always the same FD", () =>
                {
                    var s1 = _sockets[0];
                    var fd2 = s1.Options.GetInt32(O.RecvFd);
                    Assert.Equal(fd, fd2);
                });
            });
        }

        [Fact]
        public void And_they_start_non_pollable()
        {
            We_can_get_a_Recv_FD(fd =>
            {
                var @in = In.ToShort();

                Section("they start non pollable", () =>
                {
                    var x = new POLLFD((ushort) fd, @in, 0);
                    Assert.Equal(0, WsaPoll(ref x, 1, 0));
                    Assert.Equal(0, x.Revents);
                });
            });
        }

        [Fact]
        public void When_we_write_they_are_pollable()
        {
            We_can_get_a_Recv_FD(fd =>
            {
                var @in = In.ToShort();
                var rdNorm = ReaddNormal.ToShort();

                Section("when we write they are pollable", () =>
                {
                    var s2 = _sockets[1];

                    var x = new POLLFD((ushort) fd, In.ToShort(), 0);
                    using (var m = CreateMessage())
                    {
                        m.Body.Append(Kick);
                        s2.Send(m);
                        Assert.Equal(1, WsaPoll(ref x, 1, 1000));
                        // The C/C++ unit test are actually more specific than the original unit test suggests.
                        Assert.NotEqual(0, x.Revents & @in);
                        Assert.Equal(rdNorm, x.Revents & @in);
                    }
                });
            });
        }

        [Fact]
        public void We_can_get_a_Send_FD()
        {
            Given_connected_Pair_Sockets(() =>
            {
                var s1 = _sockets[0];

                Section("We can get a Send FD", () =>
                {
                    var fd = s1.Options.GetInt32(O.SendFd);
                    Assert.NotEqual(InvalidFd, fd);
                });
            });
        }

        ~PollingTests()
        {
            _sockets?[0]?.Dispose();
            _sockets?[1]?.Dispose();
        }
    }
}
