using System;

namespace Nanomsg2.Sharp
{
    // TODO: TBD: I do not think there is ever a time when we need just the Writer, so that actually works to our advantage here.
    public class OptionReaderWriter : OptionReader, IOptionReaderWriter
    {
        private SetOptDelegate<int> _setInt32;

        private SetOptDelegate<ulong> _setUInt64;

        private SetOptStringDelegate _setString;

        private SetOptDelegate<int> _setDurationMilliseconds;

        internal void SetSetters(
            SetOptDelegate<int> setInt32
            , SetOptDelegate<ulong> setUInt64
            , SetOptStringDelegate setString
            , SetOptDelegate<int> setDurationMilliseconds
        )
        {
            _setInt32 = setInt32;
            _setUInt64 = setUInt64;
            _setString = setString;
            _setDurationMilliseconds = setDurationMilliseconds;
        }

        internal OptionReaderWriter()
        {
            SetSetters(
                delegate { throw new NotImplementedException(); },
                delegate { throw new NotImplementedException(); },
                delegate { throw new NotImplementedException(); },
                delegate { throw new NotImplementedException(); }
            );
        }

        public virtual void SetString(string name, string s)
        {
            var sz = (ulong) s.Length;
            _setString(name, s, sz);
        }

        public virtual void SetInt32(string name, int value)
        {
            _setInt32(name, value);
        }

        public virtual void SetSize(string name, ulong sz)
        {
            _setUInt64(name, sz);
        }

        public virtual void SetDuration(string name, TimeSpan value)
        {
            SetTotalMilliseconds(name, (int) value.TotalMilliseconds);
        }

        public virtual void SetTotalMilliseconds(string name, int value)
        {
            _setDurationMilliseconds(name, value);
        }
    }
}
