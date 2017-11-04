using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Protocols.Survey
{
    using static Imports;
    using static UnmanagedType;

    namespace V0
    {
        public class RespondentSocket : Socket, IRespondentSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_respondent0_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public RespondentSocket()
                : base(__Open)
            {
            }
        }
    }

    public class LatestRespondentSocket : V0.RespondentSocket
    {
    }
}
