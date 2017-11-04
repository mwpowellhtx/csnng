using System.Runtime.InteropServices;

namespace Nanomsg2.Sharp.Protocols.Survey
{
    using static Imports;
    using static UnmanagedType;

    namespace V0
    {
        public class SurveyorSocket : Socket, ISurveyorSocket
        {
            [DllImport(NanomsgDll, EntryPoint = "nng_surveyor0_open", CallingConvention = Cdecl)]
            [return: MarshalAs(I4)]
            private static extern int __Open(ref uint sid);

            public SurveyorSocket()
                : base(__Open)
            {
            }
        }
    }

    public class LatestSurveyorSocket : V0.SurveyorSocket
    {
    }
}
