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

using System;

namespace Nanomsg2.Sharp
{
    using static Math;

    public abstract class PathAddressFamilyView : AddressFamilyView<SOCKADDR>, IPathAddressFamilyView
    {
        private string _path;

        private static void SetPath(string value, out string field)
        {
            value = value ?? string.Empty;
            field = value.Substring(0, Min(128, value.Length));
        }

        public string Path
        {
            get { return _path; }
            set { SetPath(value, out _path); }
        }

        protected PathAddressFamilyView(SOCKADDR @base)
            : base(@base)
        {
        }
    }
}
