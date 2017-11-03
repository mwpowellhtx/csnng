namespace Nanomsg2.Sharp
{
    public interface IAddress : IHaveOne, IHaveAddressFamilyView
    {
        ushort Family { get; set; }
    }
}
