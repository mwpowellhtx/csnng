namespace Nanomsg2.Sharp
{
    public abstract class EndPoint : Invoker, IEndPoint
    {
        protected delegate int StartDelegate(int flags);
        protected delegate int CloseDelegate();

        private StartDelegate _start;

        private CloseDelegate _close;

        protected void SetDelegates(
            StartDelegate start
            , CloseDelegate close
        )
        {
            /* We just receive the core Delegates. Which we will
             * turn around and handle the Invocation ourselves. */
            _start = start;
            _close = close;
        }

        protected OptionReaderWriter ProtectedOptions { get; }

        public IOptionReaderWriter Options => ProtectedOptions;

        protected internal abstract uint Id { get; }

        public bool HasOne => Id != 0;

        protected EndPoint()
        {
            ProtectedOptions = new OptionReaderWriter();
        }

        public virtual void Start(int flags)
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => _start(flags));
        }

        public virtual void Close()
        {
            DefaultInvoker.InvokeWithDefaultErrorHandling(() => _close());
        }
    }
}
