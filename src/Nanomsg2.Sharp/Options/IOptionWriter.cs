using System;

namespace Nanomsg2.Sharp
{
    public interface IOptionWriter
    {
        void SetString(string name, string s);

        void SetInt32(string name, int value);

        void SetSize(string name, ulong sz);

        void SetDuration(string name, TimeSpan value);

        void SetTotalMilliseconds(string name, int value);
    }
}
