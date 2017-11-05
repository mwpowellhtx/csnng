namespace Nanomsg2.Sharp.Messaging
{
    public interface IMessagePipe : IHaveOne, ICanClose, IHaveOptions<IOptionReader>
    {
        void Set();

        void Set(Message message);

        void Reset();

        bool HasMessage { get; }
    }
}
