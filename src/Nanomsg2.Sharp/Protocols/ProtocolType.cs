////
//// Copyright (c) 2017 Michael W Powell <mwpowellhtx@gmail.com>
//// Copyright 2017 Garrett D'Amore <garrett@damore.org>
//// Copyright 2017 Capitar IT Group BV <info@capitar.com>
////
//// This software is supplied under the terms of the MIT License, a
//// copy of which should be located in the distribution where this
//// file was obtained (LICENSE.txt).  A copy of the license may also be
//// found online at https://opensource.org/licenses/MIT.
////
//
//// TODO: TBD: while interesting, the fact that so many structural internals are involved really makes this aspect a "no-go" area for C#/.NET purposes
//// TODO: TBD: in C++ that's another story, but does not easily extend into this assembly
//namespace Nanomsg2.Sharp.Protocols
//{
//    /* We need to be very careful with these numbers owing to the underlying C API.
//    On account of SWIG comprehension, we must be careful to define these precisely
//    in agreement with the C code. Also leveraging the fact that Protocol is always
//    a 16-bit unsigned field. */

//    internal enum ProtocolType : ushort
//    {
//        None = 0, // ::NNG_PROTO_NONE,

//        /* TODO: TBD: these ought to be a little more robust definitions especially
//        approaching SWIG. If not, we will revisit at a later time. The C code multiplies
//        by 16, but it's the same as shifting left. Plus C# can support that, for starters. */
//        PairV0 = 1 << 4, // ::NNG_PROTO_PAIR_V0,
//        PairV1 = 1 << 4 | 1, // ::NNG_PROTO_PAIR_V1,
//        PublisherV0 = 2 << 4, // ::NNG_PROTO_PUB_V0,
//        SubscriberV0 = 2 << 4 | 1, // ::NNG_PROTO_SUB_V0,
//        RequestorV0 = 3 << 4, // ::NNG_PROTO_REQ_V0,
//        ReplierV0 = 3 << 4 | 1, // ::NNG_PROTO_REP_V0,
//        PusherV0 = 5 << 4, // ::NNG_PROTO_PUSH_V0,
//        PullerV0 = 5 << 4 | 1, // ::NNG_PROTO_PULL_V0,
//        SurveyorV0 = 6 << 4 | 2, // ::NNG_PROTO_SURVEYOR_V0,
//        RespondentV0 = 6 << 4 | 3, // ::NNG_PROTO_RESPONDENT_V0,
//        BusV0 = 7 << 4 | 0, // ::NNG_PROTO_BUS_V0,
//        StarV0 = 100 << 4, // ::NNG_PROTO_STAR_V0,

//        /* Ditto robustness. Instead of depending on the C definitions, we will leverage our
//        C++ definitons. SWIG should be able to rename these, but we will cross that bridge
//        when we get there as well and as necessary. */
//        Pair = PairV1,
//        Publisher = PublisherV0,
//        Subscriber = SubscriberV0,
//        Requestor = RequestorV0,
//        Replier = ReplierV0,
//        Pusher = PusherV0,
//        Puller = PullerV0,
//        Surveyor = SurveyorV0,
//        Respondent = RespondentV0,
//        Bus = BusV0,
//        Star = StarV0,
//    }
//}
