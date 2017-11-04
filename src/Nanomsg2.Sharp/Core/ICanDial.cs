namespace Nanomsg2.Sharp
{
    public interface ICanDial
    {
        void Dial(string addr, SocketFlag flags);

        void Dial(string addr, Dialer d, SocketFlag flags);
    }
}
