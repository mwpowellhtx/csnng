namespace Nanomsg2.Sharp
{
    public interface IZeroTierAddressFamilyView : IAddressFamilyView, IHavePort
    {
        ulong NetworkId { get; set; }

        ulong NodeId { get; set; }
    }
}
