using System;
using System.Text;

namespace Nanomsg2.Sharp
{
    // TODO: TBD: not sure we would need the raw IntPtr based getter; most times that is supporting String, that I know of, anyhow...
    internal delegate int GetOptDelegate(string name, ref IntPtr valuePtr, ref ulong sz);

    internal delegate int GetOptStringBuilderDelegate(string name, StringBuilder valuePtr, out ulong sz);

    internal delegate int GetOptDelegate<T>(string name, ref T value);

    internal delegate int SetOptDelegate(string name, ref IntPtr valuePtr, ulong sz);

    internal delegate int SetOptStringDelegate(string name, string s, ulong sz);

    internal delegate int SetOptDelegate<in T>(string name, T value);
}
