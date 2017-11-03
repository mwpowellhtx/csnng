namespace Nanomsg2.Sharp
{
    public interface IHaveOptions<out T>
        where T : class, IOptions
    {
        T Options { get; }
    }
}
