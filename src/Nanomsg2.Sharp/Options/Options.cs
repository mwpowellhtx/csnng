//
// Copyright (c) 2017 Michael W Powell <mwpowellhtx@gmail.com>
// Copyright 2017 Garrett D'Amore <garrett@damore.org>
// Copyright 2017 Capitar IT Group BV <info@capitar.com>
//
// This software is supplied under the terms of the MIT License, a
// copy of which should be located in the distribution where this
// file was obtained (LICENSE.txt).  A copy of the license may also be
// found online at https://opensource.org/licenses/MIT.
//

namespace Nanomsg2.Sharp
{
    public static class Options
    {
        /// <summary>
        /// "socket-name"
        /// </summary>
        public const string SocketName = "socket-name";

        /// <summary>
        /// "compat:domain"
        /// </summary>
        public const string CompatDomain = "compat:domain";

        /// <summary>
        /// "raw"
        /// </summary>
        public const string Raw = "raw";

        /// <summary>
        /// "linger"
        /// </summary>
        public const string LingerDuration = "linger";

        /// <summary>
        /// "recv-buffer"
        /// </summary>
        public const string RecvBuf = "recv-buffer";

        /// <summary>
        /// "send-buffer"
        /// </summary>
        public const string SendBuf = "send-buffer";

        /// <summary>
        /// "recv-fd"
        /// </summary>
        public const string RecvFd = "recv-fd";

        /// <summary>
        /// "send-fd"
        /// </summary>
        public const string SendFd = "send-fd";

        /// <summary>
        /// "recv-timeout"
        /// </summary>
        public const string RecvTimeoutDuration = "recv-timeout";

        /// <summary>
        /// "send-timeout"
        /// </summary>
        public const string SendTimeoutDuration = "send-timeout";

        /// <summary>
        /// "local-address"
        /// </summary>
        public const string LocalAddr = "local-address";

        /// <summary>
        /// "remote-address"
        /// </summary>
        public const string RemoteAddr = "remote-address";

        /// <summary>
        /// "url"
        /// </summary>
        public const string Url = "url";

        /// <summary>
        /// "ttl-max"
        /// </summary>
        public const string TtlMax = "ttl-max";

        /// <summary>
        /// "protocol"
        /// </summary>
        public const string Protocol = "protocol";

        /// <summary>
        /// "transport"
        /// </summary>
        public const string Transport = "transport";

        /// <summary>
        /// "recv-size-max"
        /// </summary>
        public const string MaxRecvSize = "recv-size-max";

        /// <summary>
        /// "reconnect-time-min"
        /// </summary>
        public const string MinReconnectDuration = "reconnect-time-min";

        /// <summary>
        /// "reconnect-time-max"
        /// </summary>
        public const string MaxReconnectDuration = "reconnect-time-max";

        /// <summary>
        /// "pair1:polyamorous"
        /// </summary>
        public const string PairV1Poly = "pair1:polyamorous";

        /// <summary>
        /// "sub:subscribe"
        /// </summary>
        public const string SubSubscribe = "sub:subscribe";

        /// <summary>
        /// "sub:unsubscribe"
        /// </summary>
        public const string SubUnsubscribe = "sub:unsubscribe";

        /// <summary>
        /// "req:resend-time"
        /// </summary>
        public const string ReqResendDuration = "req:resend-time";

        /// <summary>
        /// "surveyor:survey-time"
        /// </summary>
        public const string SurveyorSurveyDuration = "surveyor:survey-time";
    }
}
