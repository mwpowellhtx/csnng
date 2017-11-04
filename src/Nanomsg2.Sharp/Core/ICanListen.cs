namespace Nanomsg2.Sharp
{
    public interface ICanListen
    {
        void Listen(string addr, SocketFlag flags);

        void Listen(string addr, Listener l, SocketFlag flags);
    }
}
