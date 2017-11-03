namespace Nanomsg2.Sharp
{
    public abstract class Socket
    {
        // TODO: TBD: there is really no pretty way to handle this from a C# language perspective but to expose the Id for internal use
        private int _sid;

        internal int Id => _sid;
    }
}
