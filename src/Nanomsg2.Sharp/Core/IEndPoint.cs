namespace Nanomsg2.Sharp
{
    public interface IEndPoint : IHaveOne, ICanClose, IHaveOptions<IOptionReaderWriter>
    {
    }
}
