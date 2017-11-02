namespace Nanomsg2.Sharp.Messaging
{
    public interface ICanAppend
    {
    }

    public interface ICanAppend<in T> : ICanAppend
    {
        void Append(T value);
    }

    public interface ICanAppendWithSize<in T> : ICanAppend
    {
        void Append(T value, ulong sz);
    }
}
