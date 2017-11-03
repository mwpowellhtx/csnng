using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Nanomsg2.Sharp.Transports
{
    using Xunit;
    using Xunit.Abstractions;
    using static SocketAddressFamily;

    public class AddressTests : TransportTestBase
    {
        public AddressTests(ITestOutputHelper @out)
            : base(@out)
        {
        }

        private static void VerifyAddress<TView>(IAddress addy, bool hasOne)
            where TView : class, IAddressFamilyView
        {
            Assert.NotNull(addy);
            Assert.Equal(hasOne, addy.HasOne);
            var view = addy.View;
            Assert.NotNull(view);
            Assert.True(view is TView);
        }

        private static void VerifyAddress<TView>(IAddress addy)
            where TView : class, IAddressFamilyView
        {
            VerifyAddress<TView>(addy, true);
        }

        private delegate void AddressVerificationDelegate(IAddress addr);

        private static IDictionary<ushort, AddressVerificationDelegate> VerificationSwitch { get; }

        static AddressTests()
        {
            VerificationSwitch = new ConcurrentDictionary<ushort, AddressVerificationDelegate>(
                new Dictionary<ushort, AddressVerificationDelegate>
                {
                    {(ushort) Unspecified, VerifyAddress<IUnspecifiedAddressFamilyView>},
                    {(ushort) InProcess, VerifyAddress<IInProcessAddressFamilyView>},
                    {(ushort) InterProcess, VerifyAddress<IInterProcessAddressFamilyView>},
                    {(ushort) IPv4, VerifyAddress<IIPv4AddressFamilyView>},
                    {(ushort) IPv6, VerifyAddress<IIPv6AddressFamilyView>},
                    {(ushort) ZeroTier, VerifyAddress<IZeroTierAddressFamilyView>},
                }
            );
        }

        private static void VerifyAddressSwitch(IAddress addr, ushort family)
        {
            Assert.True(VerificationSwitch.ContainsKey(family));
            var verify = VerificationSwitch[family];
            verify(addr);
        }

        private static void VerifyAddressSwitch(IAddress addr)
            => VerifyAddressSwitch(addr, addr.Family);

        [Fact]
        public void ThatDefaultAddressInitializes()
        {
            var addy = new Address();
            VerifyAddress<IUnspecifiedAddressFamilyView>(addy);
        }

        [Theory]
        [InlineData(Unspecified)]
        [InlineData(InProcess)]
        [InlineData(InterProcess)]
        [InlineData(IPv4)]
        [InlineData(IPv6)]
        [InlineData(ZeroTier)]
        public void ThatAddressMaintainsTheView(SocketAddressFamily family)
        {
            // Start from a sane baseline.
            var addy = new Address();
            VerifyAddressSwitch(addy, (ushort) Unspecified);

            // TODO: TBD: a true combinatorics verification would be better, but this will do for now.
            // TODO: TBD: https://github.com/AArnott/Xunit.Combinatorial/issues/13

            addy.Family = (ushort) family;
            VerifyAddressSwitch(addy);
        }
    }
}
