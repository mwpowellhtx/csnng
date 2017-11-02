using System.Collections.Generic;

namespace Nanomsg2.Sharp.Messaging
{
    public interface ICanTrimRight
    {
    }

    public interface ICanTrimRight<TResult> : ICanTrimRight
    {
        void TrimRight(out TResult result);
    }

    public interface ICanTrimRight<in TCount, TResult> : ICanTrimRight
    {
        void TrimRight(TCount count, out TResult result);
    }
}
