namespace Nanomsg2.Sharp.Collections.Generic
{
    public interface IFixedSizeList<T> : IFixedSizeCollection<T>
    {
        T this[int index] { get; set; }
    }
}
