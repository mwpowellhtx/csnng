using System;
using System.Linq;
using System.Text;

namespace Nanomsg2.Sharp
{
    public class OptionReader : IOptionReader
    {
        private GetOptDelegate<int> _getInt32;

        private GetOptDelegate<ulong> _getUInt64;

        private GetOptStringBuilderDelegate _getStringBuilder;

        private GetOptDelegate<int> _getDurationMilliseconds;

        internal void SetGetters(
            GetOptDelegate<int> getInt32
            , GetOptDelegate<ulong> getUInt64
            , GetOptStringBuilderDelegate getStringBuilder
            , GetOptDelegate<int> getDurationMilliseconds
        )
        {
            _getInt32 = getInt32;
            _getUInt64 = getUInt64;
            _getStringBuilder = getStringBuilder;
            _getDurationMilliseconds = getDurationMilliseconds;
        }

        private static int DefaultGetStringBuilder(string name, StringBuilder valuePtr, out ulong sz)
        {
            // This one requires special attention on account of the OUT parameter.
            throw new NotImplementedException();
        }

        internal OptionReader()
        {
            SetGetters(
                delegate { throw new NotImplementedException(); },
                delegate { throw new NotImplementedException(); },
                DefaultGetStringBuilder,
                delegate { throw new NotImplementedException(); }
            );
        }

        public virtual string GetText(string name)
        {
            ulong sz = 128;
            return GetText(name, ref sz);
        }

        public virtual string GetText(string name, ref ulong sz)
        {
            var sb = new StringBuilder((int) sz);
            _getStringBuilder(name, sb, out sz);
            var s = sb.ToString().Trim();
            sz = (ulong) s.LongCount();
            return s;
        }

        public virtual int GetInt32(string name)
        {
            var value = default(int);
            _getInt32(name, ref value);
            return value;
        }

        public virtual ulong GetSize(string name)
        {
            var value = default(ulong);
            _getUInt64(name, ref value);
            return value;
        }

        public virtual double GetTotalMilliseconds(string name)
        {
            var value = default(int);
            _getDurationMilliseconds(name, ref value);
            return value;
        }

        public virtual TimeSpan GetTimeSpan(string name)
        {
            var value = GetTotalMilliseconds(name);
            return TimeSpan.FromMilliseconds(value);
        }
    }
}
