using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flipsider.Core.Collections
{
    /// <summary>
    /// Contains an ordered set where values can be added, removed, inserted, and fetched with constant-time complexity.
    /// </summary>
    public sealed class OrderedSet<T> : ICollection<T> where T : notnull
    {
        private readonly Dictionary<T, LinkedListNode<T>> dictionary;
        private readonly LinkedList<T> linkedList;

        public OrderedSet() : this(Enumerable.Empty<T>(), EqualityComparer<T>.Default) { }
        public OrderedSet(IEqualityComparer<T> comparer) : this(Enumerable.Empty<T>(), comparer) { }
        public OrderedSet(IEnumerable<T> objects) : this(objects, EqualityComparer<T>.Default) { }
        public OrderedSet(IEnumerable<T> objects, IEqualityComparer<T> comparer)
        {
            dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
            linkedList = new LinkedList<T>();
            foreach (var item in objects)
            {
                Add(item);
            }
        }

        public int Count => dictionary.Count;

        bool ICollection<T>.IsReadOnly => false;

        void ICollection<T>.Add(T item) => Add(item);

        public bool Add(T item)
        {
            if (!dictionary.ContainsKey(item))
            {
                dictionary.Add(item, linkedList.AddLast(item));
                return true;
            }
            return false;
        }

        public bool InsertAfter(T anchor, T item)
        {
            if (dictionary.TryGetValue(anchor, out var val))
            {
                linkedList.AddAfter(val, item);
                return true;
            }
            return false;
        }

        public bool InsertBefore(T anchor, T item)
        {
            if (dictionary.TryGetValue(anchor, out var val))
            {
                linkedList.AddBefore(val, item);
                return true;
            }
            return false;
        }

        public bool Remove(T item)
        {
            if (dictionary.TryGetValue(item, out var node))
            {
                dictionary.Remove(item);
                linkedList.Remove(node);
                return true;
            }
            return false;
        }

        public bool Contains(T item) => dictionary.ContainsKey(item);

        void ICollection<T>.Clear()
        {
            linkedList.Clear();
            dictionary.Clear();
        }

        public IEnumerator<T> GetEnumerator() => linkedList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => linkedList.CopyTo(array, arrayIndex);
    }
}
