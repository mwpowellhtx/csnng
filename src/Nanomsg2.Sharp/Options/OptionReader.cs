//
// Copyright (c) 2017 Michael W Powell <mwpowellhtx@gmail.com>
// Copyright 2017 Garrett D'Amore <garrett@damore.org>
// Copyright 2017 Capitar IT Group BV <info@capitar.com>
//
// This software is supplied under the terms of the MIT License, a
// copy of which should be located in the distribution where this
// file was obtained (LICENSE.txt).  A copy of the license may also be
// found online at https://opensource.org/licenses/MIT.
//

using System;
using System.Linq;
using System.Text;

namespace Nanomsg2.Sharp
{
    public class OptionReader : Invoker, IOptionReader
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
            _configured = true;
        }

        private bool _configured;

        public virtual bool HasOne => _configured;

        internal OptionReader()
        {
            SetGetters(
                delegate { throw ThrowInvalidOperation(nameof(_getInt32)); }
                , delegate { throw ThrowInvalidOperation(nameof(_getUInt64)); }
                , delegate { throw ThrowInvalidOperation(nameof(_getStringBuilder)); }
                , delegate { throw ThrowInvalidOperation(nameof(_getDurationMilliseconds)); }
            );
            _configured = false;
        }

        public virtual string GetText(string name)
        {
            ulong sz = 128;
            return GetText(name, ref sz);
        }

        public virtual string GetText(string name, ref ulong length)
        {
            var sz = length;
            var sb = new StringBuilder((int) length);
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => _getStringBuilder(name, sb, ref sz));
            var s = sb.ToString().Trim();
            length = (ulong) s.LongCount();
            return s;
        }

        public virtual int GetInt32(string name)
        {
            var value = default(int);
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => _getInt32(name, ref value));
            return value;
        }

        public virtual ulong GetSize(string name)
        {
            var value = default(ulong);
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => _getUInt64(name, ref value));
            return value;
        }

        public virtual TimeSpan GetTimeSpan(string name)
        {
            var value = GetTotalMilliseconds(name);
            return TimeSpan.FromMilliseconds(value);
        }

        public virtual double GetTotalMilliseconds(string name)
        {
            var value = default(int);
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => _getDurationMilliseconds(name, ref value));
            return value;
        }
    }
}
