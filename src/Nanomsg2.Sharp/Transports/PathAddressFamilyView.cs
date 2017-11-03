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
