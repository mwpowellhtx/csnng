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

namespace Nanomsg2.Sharp.Async
{
    using Protocols.Pair;
    using Xunit;
    using Xunit.Abstractions;
    using static TimeSpan;
    using static ErrorCode;
    using static SocketAddressFamily;
    using O = Options;

    public class AsyncTests : BehaviorDrivenTestFixtureBase
    {
        private string TestAddr
        {
            get
            {
                var addr = InProcess.BuildAddress<AsyncTests>();
                Out.WriteLine($"Testing using address '{addr}'");
                return addr;
            }
        }

        private const string Hello = "hello";

        public AsyncTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        private delegate void ConnectedSocketsCallback(Socket s1, Socket s2);

        private void Given_two_connected_sockets(ConnectedSocketsCallback callback)
        {
            var addr = TestAddr;

            Section($"given two connected sockets", () =>
            {
                LatestPairSocket s1 = null;
                LatestPairSocket s2 = null;

                try
                {
                    s1 = CreateOne<LatestPairSocket>();
                    s2 = CreateOne<LatestPairSocket>();

                    s1.Listen(addr);
                    s2.Dial(addr);

                    callback(s1, s2);
                }
                finally
                {
                    DisposeAll(s1, s2);
                }
            });
        }

        [Fact]
        public void Send_and_recv_works()
        {
            const double timeout = 100d;

            Given_two_connected_sockets((s1, s2) =>
            {
                Section($"async send and receive works", () =>
                {
                    int txSurplus = 0, rxDeficit = 1;

                    // Using the same Message throughout the body of this unit test.
                    using (var m = CreateMessage())
                    {
                        // Make sure that we are disposing the Services afterward.
                        BasicAsyncService txSvc = null;
                        BasicAsyncService rxSvc = null;

                        try
                        {
                            m.Body.Append(Hello);

                            // TODO: TBD: could have a base test class with CreateAsyncService...
                            // This is the kind of economy we want. Increased surplus, decreased deficit.
                            txSvc = new BasicAsyncService(() => txSurplus++);
                            rxSvc = new BasicAsyncService(() => --rxDeficit);

                            Assert.True(txSvc.HasOne);
                            Assert.True(rxSvc.HasOne);

                            txSvc.Options.SetTimeoutDuration(FromMilliseconds(timeout));
                            rxSvc.Options.SetTimeoutDuration(FromMilliseconds(timeout));

                            Assert.Equal(0, txSurplus);
                            Assert.Equal(1, rxDeficit);

                            txSvc.Retain(m);
                            Assert.False(m.HasOne);

                            s2.ReceiveAsync(rxSvc);
                            s1.SendAsync(txSvc);

                            rxSvc.Wait();

                            Assert.True(txSvc.Success);
                            Assert.True(rxSvc.Success);

                            rxSvc.Cede(m);
                            Assert.True(m.HasOne);

                            Assert.Equal(Hello.ToBytes(), m.Body.Get());
                        }
                        finally
                        {
                            DisposeAll(txSvc, rxSvc);
                        }
                    }

                    // We like this kind of deficit spending.
                    Assert.Equal(1, txSurplus);
                    Assert.Equal(0, rxDeficit);
                });
            });
        }

        private const double NominalTimeout = 40d;

        private delegate void FailureModeDelegate(Socket s, BasicAsyncServiceFixture svc);

        private void Failure_mode_assumptions(FailureModeDelegate callback)
        {
            Section("failure modes work", () =>
            {
                LatestPairSocket s = null;

                // We will leverage the Fixtured Done field.
                BasicAsyncServiceFixture svc = null;

                try
                {
                    s = CreateOne<LatestPairSocket>();

                    // TODO: TBD: ditto CreateAsyncService...
                    svc = new BasicAsyncServiceFixture();
                    svc.Start(() => ++svc.Done);

                    Assert.Equal(0, svc.Done);

                    callback(s, svc);
                }
                finally
                {
                    DisposeAll(s, svc);
                }
            });
        }

        private static void WaitForDoneAndSuccess(BasicAsyncServiceFixture svc, ErrorCode ec)
        {
            svc.Wait();
            Assert.Equal(1, svc.Done);
            Assert.Throws<NanoException>(() => svc.Success)
                .Matching(ex => ex.ErrorNumber.ToErrorCode() == ec);
        }

        [Fact]
        public void Service_timeout_works()
        {
            Failure_mode_assumptions((s, svc) =>
            {
                Section("Service timeout works", () =>
                {
                    svc.Options.SetTimeoutDuration(FromMilliseconds(NominalTimeout));
                    s.ReceiveAsync(svc);
                    WaitForDoneAndSuccess(svc, TimedOut);
                });
            });
        }

        [Fact]
        public void Socket_timeout_works()
        {
            Failure_mode_assumptions((s, svc) =>
            {
                Section("Socket timeout works", () =>
                {
                    s.Options.SetDuration(O.RecvTimeoutDuration, FromMilliseconds(NominalTimeout));
                    s.ReceiveAsync(svc);
                    WaitForDoneAndSuccess(svc, TimedOut);
                });
            });
        }

        [Fact]
        public void Zero_timeout_works()
        {
            Failure_mode_assumptions((s, svc) =>
            {
                Section("Zero timeout works", () =>
                {
                    svc.Options.SetTimeout(Duration.Zero);
                    s.ReceiveAsync(svc);
                    WaitForDoneAndSuccess(svc, TimedOut);
                });
            });
        }

        [Fact]
        public void Cancellation_works()
        {
            Failure_mode_assumptions((s, svc) =>
            {
                Section("Cancellation works", () =>
                {
                    svc.Options.SetTimeout(Duration.Infinite);
                    s.ReceiveAsync(svc);
                    svc.Cancel();
                    WaitForDoneAndSuccess(svc, Canceled);
                });
            });
        }
    }
}
