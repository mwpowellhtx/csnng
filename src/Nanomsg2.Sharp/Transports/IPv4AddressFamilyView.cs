namespace Nanomsg2.Sharp
{
    public class IPv4AddressFamilyView : AddressFamilyView<SOCKADDR>, IIPv4AddressFamilyView
    {
        public override ushort Family => (ushort) SocketAddressFamily.IPv4;

        public uint Address { get; set; }

        public ushort Port { get; set; }

        internal IPv4AddressFamilyView(ref SOCKADDR @base)
            : base(@base)
        {
            Address = @base.IPv4.Address;
            Port = @base.IPv4.Port;
        }
    }
}
