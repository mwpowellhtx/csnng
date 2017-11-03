namespace Nanomsg2.Sharp
{
    public interface IIPv4AddressFamilyView : IAddressFamilyView, IHavePort
    {
        uint Address { get; set; }
    }
}
