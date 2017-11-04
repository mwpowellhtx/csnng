using System;

namespace Nanomsg2.Sharp.Protocols.Survey
{
    using Messaging;
    using Xunit;
    using Xunit.Abstractions;
    using static TimeSpan;
    using static ErrorCode;
    using O = Options;

    public class SurveyTests : ProtocolTestBase
    {
        private const string Abc = "abc";
        private const string Def = "def";

        public SurveyTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        [Fact]
        public void Surveyor_receive_with_no_Respondent_fails()
        {
            Given_default_socket<LatestSurveyorSocket>(s =>
            {
                Section("receive with no respondent fails", () =>
                {
                    Message m = null;
                    try
                    {
                        m = CreateMessage();

                        Assert.Throws<NanoException>(() => s.TryReceive(m))
                            .Matching(ex => ex.ErrorNumber.ToErrorCode() == State);
                    }
                    catch
                    {
                        DisposeAll(m);
                    }
                });
            });
        }

        [Fact]
        public void Surveyor_with_no_Respondent_times_out()
        {
            Given_default_socket<LatestSurveyorSocket>(s =>
            {
                Section("survey with no respondent times out", () =>
                {
                    Message m = null;
                    try
                    {
                        m = CreateMessage();

                        s.Options.SetDuration(O.SurveyorSurveyDuration, FromMilliseconds(50d));

                        s.Send(m);

                        Assert.Throws<NanoException>(() => s.TryReceive(m))
                            .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);
                    }
                    catch
                    {
                        DisposeAll(m);
                    }
                });
            });
        }

        [Fact]
        public void Respondent_send_with_no_Surveyor_fails()
        {
            Given_default_socket<LatestRespondentSocket>(s =>
            {
                Section("send with no surveyor fails", () =>
                {
                    Message m = null;
                    try
                    {
                        m = CreateMessage();

                        Assert.Throws<NanoException>(() => s.Send(m))
                            .Matching(ex => ex.ErrorNumber.ToErrorCode() == State);
                    }
                    catch
                    {
                        DisposeAll(m);
                    }
                });
            });
        }

        private delegate void LinkedSocketsCallback(LatestSurveyorSocket sur, LatestRespondentSocket resp);

        private void We_can_create_linked_Sockets(LinkedSocketsCallback callback)
        {
            var addr = TestAddr;

            Section("We can create linked Sockets", () =>
            {
                LatestSurveyorSocket sur = null;
                LatestRespondentSocket resp = null;

                try
                {
                    sur = CreateOne<LatestSurveyorSocket>();
                    resp = CreateOne<LatestRespondentSocket>();

                    sur.Options.SetDuration(O.SurveyorSurveyDuration, FromMilliseconds(50d));

                    sur.Listen(addr);
                    resp.Dial(addr);

                    using (var s = CreateOne<LatestRespondentSocket>())
                    {
                        s.Dial(addr);
                    }

                    callback(sur, resp);
                }
                finally
                {
                    DisposeAll(sur, resp);
                }
            });
        }

        [Fact]
        public void Survey_works()
        {
            We_can_create_linked_Sockets((sur, resp) =>
            {
                Section("survey works", () =>
                {
                    Message m = null;

                    try
                    {
                        m = CreateMessage();

                        m.Body.Append(Abc);
                        sur.Send(m);
                        Assert.True(resp.TryReceive(m));
                        Assert.Equal(Abc.ToBytes(), m.Body.Get());

                        m.Clear();
                        m.Body.Append(Def);
                        resp.Send(m);
                        Assert.True(sur.TryReceive(m));
                        Assert.Equal(Def.ToBytes(), m.Body.Get());

                        Section("Surveyor times out receiving again", () =>
                        {
                            Assert.Throws<NanoException>(() => sur.TryReceive(m))
                                .Matching(ex => ex.ErrorNumber.ToErrorCode() == TimedOut);

                            Section("Surveyor goes to non survey state", () =>
                            {
                                sur.Options.SetDuration(O.RecvTimeoutDuration, FromMilliseconds(200d));

                                Assert.Throws<NanoException>(() => sur.TryReceive(m))
                                    .Matching(ex => ex.ErrorNumber.ToErrorCode() == State);
                            });
                        });
                    }
                    catch
                    {
                        DisposeAll(m);
                    }
                });
            });
        }
    }
}
