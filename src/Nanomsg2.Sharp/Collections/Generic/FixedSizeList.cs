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
using System.Collections.Generic;
using System.Linq;

namespace Nanomsg2.Sharp.Collections.Generic
{
    public class FixedSizeList<T> : FixedSizeCollection<T>, IFixedSizeList<T>
    {
        private static IEnumerable<T> CreateValues(int size)
        {
            return Enumerable.Range(0, size).Select(x => default(T));
        }

        private readonly IList<T> _values;

        internal FixedSizeList(int size)
            : this(CreateValues(size).ToList())
        {
        }

        internal FixedSizeList(IEnumerable<T> values)
            : this(values.ToList())
        {
        }

        internal FixedSizeList(params T[] values)
            : this(values.ToList())
        {
        }

        internal FixedSizeList(IList<T> values)
            : base(values.ToList())
        {
            _values = values;
        }

        private void ListAction(Action<IList<T>> action)
        {
            action(_values);
        }

        private TResult ListFunc<TResult>(Func<IList<T>, TResult> func)
        {
            return func(_values);
        }

        public override void Clear() => ListAction(x =>
        {
            for (var i = 0; i < x.Count; i++)
            {
                x[i] = default(T);
            }
        });

        public T this[int index]
        {
            get { return ListFunc(x => x[index]); }
            set { ListAction(x => x[index] = value); }
        }
    }
}
