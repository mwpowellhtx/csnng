namespace Nanomsg2.Sharp
{
    public class ZeroTierAddressFamilyView : AddressFamilyView<SOCKADDR>, IZeroTierAddressFamilyView
    {

        public override ushort Family => (ushort) SocketAddressFamily.ZeroTier;

        public ulong NetworkId { get; set; }

        public ulong NodeId { get; set; }

        public ushort Port { get; set; }

        internal ZeroTierAddressFamilyView(ref SOCKADDR @base)
            : base(@base)
        {
            NetworkId = @base.ZeroTier.NetworkId;
            NodeId = @base.ZeroTier.NodeId;
            Port = @base.ZeroTier.Port;
        }
    }
}
