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
            _configured = true;
        }

        private bool _configured;

        internal OptionReaderWriter()
        {
            SetSetters(
                delegate { throw ThrowInvalidOperation(nameof(_setInt32)); }
                , delegate { throw ThrowInvalidOperation(nameof(_setUInt64)); }
                , delegate { throw ThrowInvalidOperation(nameof(_setString)); }
                , delegate { throw ThrowInvalidOperation(nameof(_setDurationMilliseconds)); }
            );
            _configured = false;
        }

        public override bool HasOne => _configured && base.HasOne;

        public virtual void SetString(string name, string s)
        {
            var sz = (ulong) s.Length;
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => _setString(name, s, sz));
        }

        public virtual void SetInt32(string name, int value)
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => _setInt32(name, value));
        }

        public virtual void SetSize(string name, ulong sz)
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => _setUInt64(name, sz));
        }

        public virtual void SetDuration(string name, TimeSpan value)
        {
            SetTotalMilliseconds(name, (int) value.TotalMilliseconds);
        }

        public virtual void SetTotalMilliseconds(string name, int value)
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => _setDurationMilliseconds(name, value));
        }
    }
}
