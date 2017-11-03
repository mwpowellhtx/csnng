namespace Nanomsg2.Sharp
{
    public class UnspecifiedAddressFamilyView : AddressFamilyView<SOCKADDR>, IUnspecifiedAddressFamilyView
    {
        public override ushort Family => (ushort) SocketAddressFamily.Unspecified;

        internal UnspecifiedAddressFamilyView(ref SOCKADDR @base)
            : base(@base)
        {
        }
    }
}
