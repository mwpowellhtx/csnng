namespace Nanomsg2.Sharp.Messaging
{
    public interface ICanPrepend
    {
    }

    public interface ICanPrepend<in T> : ICanPrepend
    {
        void Prepend(T value);
    }


    public interface ICanPrependWithSize<in T> : ICanAppend
    {
        void Prepend(T value, ulong sz);
    }
}
