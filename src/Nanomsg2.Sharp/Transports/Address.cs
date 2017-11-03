using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Nanomsg2.Sharp
{
    using static SocketAddressFamily;

    public class Address : IAddress
    {
        private SOCKADDR _addr;

        public ushort Family { get; set; }

        private IAddressFamilyView _view;

        private delegate IAddressFamilyView CreateAddressFamilyViewFactory(ref SOCKADDR sa);

        private static IDictionary<ushort, CreateAddressFamilyViewFactory> Factories { get; }

        static Address()
        {
            Factories = new ConcurrentDictionary<ushort, CreateAddressFamilyViewFactory>(
                new Dictionary<ushort, CreateAddressFamilyViewFactory>
                {
                    {(ushort) Unspecified, (ref SOCKADDR sa) => new UnspecifiedAddressFamilyView(ref sa)},
                    {(ushort) InProcess, (ref SOCKADDR sa) => new InProcessAddressFamilyView(ref sa)},
                    {(ushort) InterProcess, (ref SOCKADDR sa) => new InterProcessAddressFamilyView(ref sa)},
                    {(ushort) IPv4, (ref SOCKADDR sa) => new IPv4AddressFamilyView(ref sa)},
                    {(ushort) IPv6, (ref SOCKADDR sa) => new IPv6AddressFamilyView(ref sa)},
                    {(ushort) ZeroTier, (ref SOCKADDR sa) => new ZeroTierAddressFamilyView(ref sa)},
                }
            );
        }

        private IAddressFamilyView CalculateView(ref IAddressFamilyView view)
        {
            /* Remember this is simply a View into the underlying SOCKADDR.
             *  We will spit out a brand new SOCKADDR afterwards. */

            var x = Family;
            var factories = Factories;

            // ReSharper disable once InvertIf
            if (view?.Family != x)
            {
                const ushort unspec = (ushort) Unspecified;
                var factory = factories[factories.ContainsKey(x) ? x : unspec];
                view = factory(ref _addr);
            }

            return view;
        }

        public IAddressFamilyView View => CalculateView(ref _view);

        public Address()
        {
            _addr = new SOCKADDR();
        }

        public Address(SocketAddressFamily family)
            : this((ushort) family)
        {
        }

        public Address(ushort family)
            : this()
        {
            _addr.Family = Family = family;
        }

        public bool HasOne => false == ReferenceEquals(View, null);
    }
}
