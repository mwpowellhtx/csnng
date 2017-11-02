namespace Nanomsg2.Sharp.Messaging
{
    public interface ICanTrimRight
    {
    }

    public interface ICanTrimRight<T> : ICanTrimRight
    {
        void TrimRight(out T value);
    }

    public interface ICanTrimBytesRight<in T> : ICanTrimRight
    {
        void TrimRight(T sz);
    }
}
