using System;

namespace Nanomsg2.Sharp
{
    public interface IOptionReader : IOptions
    {
        // TODO: TBD: not sure if we would really need/want to expose the IntPtr based getter

        string GetText(string name);

        string GetText(string name, ref ulong length);

        int GetInt32(string name);

        ulong GetSize(string name);

        double GetTotalMilliseconds(string name);

        TimeSpan GetTimeSpan(string name);

        //// TODO: TBD: add the Address
        //IAddress GetAddress(string name);
    }
}
