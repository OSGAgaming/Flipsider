using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Flipsider.Core.Collections
{
    /// <summary>
    /// Contains a set of objects that may be collected by GC.
    /// </summary>
    /// <typeparam name="T">The type of objects to keep weak references to.</typeparam>
    public sealed class WeakObjectSet<T> : ICollection<T>
    {
        private readonly HashSet<GCHandle> items;

        public WeakObjectSet() => items = new HashSet<GCHandle>();
        public WeakObjectSet(int capacity) => items = new HashSet<GCHandle>(capacity);

        private GCHandle From(T obj)
        {
            return GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection);
        }

        int ICollection<T>.Count => items.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            items.Add(From(item));
        }

        public void Clear()
        {
            foreach (var item in items)
                item.Free();
            items.Clear();
        }

        public void ClearDead()
        {
            items.RemoveWhere(g =>
            {
                if (g.Target is T val)
                    return false;
                g.Free();
                return true;
            });
        }

        public void RemoveWhere(Predicate<T> predicate)
        {
            items.RemoveWhere(g =>
            {
                if (g.Target is T val)
                    return predicate(val);
                g.Free();
                return true;
            });
        }

        public bool Contains(T item)
        {
            return items.Contains(From(item));
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            using var enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
                if (enumerator.Current.Target is T value)
                    yield return value;
        }

        public bool Remove(T item)
        {
            var handle = From(item);
            handle.Free();
            return items.Remove(handle);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ~WeakObjectSet()
        {
            items.Clear();
        }
    }
}
