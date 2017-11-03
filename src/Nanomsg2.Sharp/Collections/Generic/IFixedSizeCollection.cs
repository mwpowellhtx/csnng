namespace Nanomsg2.Sharp.Collections.Generic
{
    public interface IFixedSizeCollection<in T>
    {
        int Size { get; }

        int Count { get; }

        bool IsReadOnly { get; }

        void Clear();

        void CopyTo(T[] array, int arrayIndex);
    }
}
