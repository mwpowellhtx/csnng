using System.Collections.Generic;

namespace Nanomsg2.Sharp.Messaging
{
    public interface ICanTrimLeft
    {
    }

    // TODO: TBD: consider consolidating trim left/right: need parameters such as count or sz, and the out value
    // TODO: TBD: this sounds like a decent comprehension of the C API, but save for just after establishing VerCtrl baseline
    public interface ICanTrimLeft<in TCount, TResult> : ICanTrimLeft
    {
        void TrimLeft(TCount count, out TResult result);
    }
}
