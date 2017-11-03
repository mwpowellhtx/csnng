namespace Nanomsg2.Sharp
{
    public interface IPathAddressFamilyView : IAddressFamilyView
    {
        string Path { get; set; }
    }
}
