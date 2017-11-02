namespace Nanomsg2.Sharp.Messaging
{
    using Xunit;

    // TODO: TBD: establish a Message-only set of unit tests
    // TODO: TBD: establish messagepart-base class tests, and header- and body- specific unit tests.
    // TODO: TBD: should be able to factor that much better...
    public class MessageTests : MessageTestBase
    {
        [Theory]
        [InlineData(0)]
        [InlineData(0x10)]
        [InlineData(0x100)]
        [InlineData(0x1000)]
        public void CanCreateMessage(ulong sz)
        {
            VerifyMessage(sz, m =>
            {
                VerifyHavingOne(m, true);
                VerifyHavingOne(m.Header, m.HasOne);
                VerifyHavingOne(m.Body, m.HasOne);

                VerifySize(m.Header, 0ul);
                VerifySize(m.Body, sz);
                VerifySize(m, sz);
            });
        }
    }
}
