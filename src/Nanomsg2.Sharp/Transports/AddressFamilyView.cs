namespace Nanomsg2.Sharp
{
    public abstract class AddressFamilyView<TBase> : IAddressFamilyView
        where TBase : struct
    {
        protected TBase Base { get; }

        public abstract ushort Family { get; }

        protected AddressFamilyView(TBase @base)
        {
            Base = @base;
        }
    }
}
