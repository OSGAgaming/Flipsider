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

        /// <summary>
        /// Adds an item to the end of the <see cref="OrderedSet{T}"/> if it isn't already present.
        /// </summary>
        /// <param name="item">The element to add.</param>
        /// <returns>True if the operation was successful; false if the item was already present.</returns>
        public bool Add(T item)
        {
            return dictionary.TryAdd(item, linkedList.AddLast(item));
        }

        /// <summary>
        /// Adds an item to the beginning of the <see cref="OrderedSet{T}"/> if it isn't already present.
        /// </summary>
        /// <param name="item">The element to add.</param>
        /// <returns>True if the operation was successful; false if the item was already present.</returns>
        public bool AddFirst(T item)
        {
            return dictionary.TryAdd(item, linkedList.AddFirst(item));
        }

        /// <summary>
        /// Adds an item after the <paramref name="anchor"/> if the item isn't already present and the anchor exists.
        /// </summary>
        /// <param name="item">The element to add.</param>
        /// <returns>True if the operation was successful; false if the item was already present or the anchor did not exist.</returns>
        public bool AddAfter(T anchor, T item)
        {
            if (dictionary.TryGetValue(anchor, out var val))
            {
                return dictionary.TryAdd(item, linkedList.AddAfter(val, item));
            }
            return false;
        }

        /// <summary>
        /// Adds an item before the <paramref name="anchor"/> if the item isn't already present and the anchor exists.
        /// </summary>
        /// <param name="item">The element to add.</param>
        /// <returns>True if the operation was successful; false if the item was already present or the anchor did not exist.</returns>
        public bool AddBefore(T anchor, T item)
        {
            if (dictionary.TryGetValue(anchor, out var val))
            {
                return dictionary.TryAdd(item, linkedList.AddBefore(val, item));
            }
            return false;
        }

        /// <summary>
        /// Removes an item from the <see cref="OrderedSet{T}"/> if it's present.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>True if the operation was successful; false if the item was not present.</returns>
        public bool Remove(T item)
        {
            if (dictionary.Remove(item, out var node))
            {
                linkedList.Remove(node);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a copy of this collection in the form of a dictionary of linked list nodes.
        /// </summary>
        public Dictionary<T, LinkedListNode<T>> ToDictionary() => new Dictionary<T, LinkedListNode<T>>(dictionary);

        /// <summary>
        /// Determines whether the value is present in the <see cref="OrderedSet{T}"/>.
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>True if the item was present; false otherwise</returns>
        public bool Contains(T item) => dictionary.ContainsKey(item);

        /// <summary>
        /// Removes all items from the <see cref="OrderedSet{T}"/>. Clearing is not an O(1) operation.
        /// </summary>
        public void Clear()
        {
            linkedList.Clear();
            dictionary.Clear();
        }

        /// <summary>
        /// Gets an enumerator for the <see cref="OrderedSet{T}"/>. Enumerating is not an O(1) operation.
        /// </summary>
        public IEnumerator<T> GetEnumerator() => linkedList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => linkedList.CopyTo(array, arrayIndex);
    }
}
