using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Messaging
{
    public abstract class MessagePart : IMessagePart
    {
        protected Message ProtectedParent { get; }

        public IMessage Parent => ProtectedParent;

        protected MessagePart(Message parent)
        {
            ProtectedParent = parent;
        }

        public bool HasOne => Parent?.HasOne ?? false;

        public abstract ulong Size { get; }

        public abstract void Clear();

        protected virtual byte[] DecodeGetResult(IntPtr data)
        {
            /* There is no way to conveniently Marshal a return variable,
             *  so we received it as an IntPtr and must now decode it. */
            var result = new byte[data == IntPtr.Zero ? 0 : Size];
            if (!result.Any()) return result;
            Marshal.Copy(data, result, 0, (int)Size);
            return result;
        }

        public abstract byte[] Get();

        public abstract void Append(uint value);
        public abstract void Prepend(uint value);
        public abstract void TrimLeft(out uint value);
        public abstract void TrimRight(out uint value);

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

        public virtual void TrimLeft(ulong sz)
        {
            throw new NotImplementedException();
        }

        public virtual void TrimRight(ulong sz)
        {
            throw new NotImplementedException();
        }
    }
}
