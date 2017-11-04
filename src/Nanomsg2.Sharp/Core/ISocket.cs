namespace Nanomsg2.Sharp
{
    public interface ISocket
        : IHaveOne, ICanClose
            , ICanListen, ICanDial
            , ISender, IReceiver
            , IHaveOptions<IOptionReaderWriter>
    {
    }
}
