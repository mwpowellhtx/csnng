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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Messaging
{
    public abstract class MessagePart : IMessagePart
    {
        public IMessage Parent { get; }

        protected Message Invoker => Parent as Message;

        protected MessagePart(IMessage parent)
        {
            Parent = parent;
        }

        public bool HasOne => Parent?.HasOne ?? false;

        public abstract ulong Size { get; }

        public abstract void Clear();

        protected virtual IEnumerable<byte> DecodeGetResult(IntPtr data)
        {
            /* There is no way to conveniently Marshal a return variable,
             *  so we received it as an IntPtr and must now decode it. */
            var result = new byte[data == IntPtr.Zero ? 0 : Size];
            if (!result.Any()) return result;
            Marshal.Copy(data, result, 0, (int)Size);
            return result;
        }

        public abstract IEnumerable<byte> Get();

        public abstract void Append(uint value);
        public abstract void Prepend(uint value);

        protected abstract uint TrimLeft();

        public void TrimLeft(out uint result)
        {
            result = TrimLeft();
        }

        public void TrimLeft(int count, out IEnumerable<uint> result)
        {
            // TODO: TBD: ditto TrimRight
            var local = new List<uint>();
            while (local.Count < count)
            {
                local.Add(TrimLeft());
            }
            result = local.ToArray();
        }

        protected abstract uint TrimRight();

        public void TrimRight(out uint result)
        {
            result = TrimRight();
        }

        public void TrimRight(int count, out IEnumerable<uint> result)
        {
            /* TODO: TBD: we could get highly functional here into a "general" case, and maybe
             * we will, but for now I do not see the cost/benefit being worth it without
             * needlessly complicating things. */
            var local = new List<uint>();
            while (local.Count < count)
            {
                local.Insert(0, TrimRight());
            }
            result = local.ToArray();
        }

        public virtual void Append(IEnumerable<byte> buffer, ulong sz)
        {
            throw new NotImplementedException();
        }

        public virtual void Append(IEnumerable<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public virtual void Append(string s)
        {
            throw new NotImplementedException();
        }

        public virtual void Prepend(IEnumerable<byte> buffer, ulong sz)
        {
            throw new NotImplementedException();
        }

        public virtual void Prepend(IEnumerable<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public virtual void Prepend(string s)
        {
            throw new NotImplementedException();
        }

        public virtual void TrimLeft(ulong sz, out IEnumerable<byte> result)
        {
            throw new NotImplementedException();
        }

        public virtual void TrimRight(ulong sz, out IEnumerable<byte> result)
        {
            throw new NotImplementedException();
        }

        public virtual void TrimLeft(int length, out string result)
        {
            throw new NotImplementedException();
        }

        public virtual void TrimRight(int length, out string result)
        {
            throw new NotImplementedException();
        }
    }
}
