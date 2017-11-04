namespace Nanomsg2.Sharp
{
    public interface ISameAs<in T>
    {
        bool SameAs(T other);
    }
}
