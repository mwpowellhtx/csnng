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

namespace Nanomsg2.Sharp.Collections.Generic
{
    public abstract class FixedSizeCollection<T> : IFixedSizeCollection<T>
    {
        private readonly ICollection<T>_collection;

        protected FixedSizeCollection(ICollection<T> collection)
        {
            _collection = collection;
        }

        private void CollectionAction(Action<ICollection<T>> action)
        {
            action(_collection);
        }

        private TResult CollectionFunc<TResult>(Func<ICollection<T>, TResult> func)
        {
            return func(_collection);
        }

        public int Size => Count;

        public virtual int Count
            => CollectionFunc(x => x.Count);

        public virtual bool IsReadOnly
            => CollectionFunc(x => x.IsReadOnly);

        public abstract void Clear();

        public virtual void CopyTo(T[] array, int arrayIndex)
            => CollectionAction(x => x.CopyTo(array, arrayIndex));
    }
}
