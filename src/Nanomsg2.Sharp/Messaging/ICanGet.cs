namespace Nanomsg2.Sharp.Messaging
{
    public interface ICanGet
    {
    }

    public interface ICanGet<out T> : ICanGet
    {
        T Get();
    }
}
