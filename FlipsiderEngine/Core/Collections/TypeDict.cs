using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Flipsider.Core.Collections
{
    /// <summary>
    /// A purely compile-time collection of values corresponding to types.
    /// </summary>
    public sealed class TypeDict : IServiceProvider
    {
        // Hold a weak pointer to this object. Lets it be collected by GC.
        private GCHandle handle;
        // Store a cache of "cleanup tasks". Since you can't iterate compile-time generics, ya store it in a delegate instead.
        private HashSet<Action> ondeath = new HashSet<Action>();

        /// <summary>
        /// Initializes a new TypeDict.
        /// </summary>
        public TypeDict()
        {
            handle = GCHandle.Alloc(this, GCHandleType.Weak);
        }

        /// <summary>
        /// Try to get the value corresponding with a type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The returned value, if any.</param>
        /// <returns>True if the value is found; false otherwise.</returns>
        public bool TryGet<T>([MaybeNullWhen(false)] out T value)
        {
            return KeyValue<T>.typeDictInstances.TryGetValue(handle, out value);
        }

        /// <summary>
        /// Get the value corresponding with a type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The type.</returns>
        /// <exception cref="KeyNotFoundException">If no match is found, this is thrown.</exception>
        public T Get<T>()
        {
            if (TryGet(out T ret))
                return ret;
            throw new KeyNotFoundException("Type was not added to the type dictionary.");
        }

        /// <summary>
        /// Sets the value corresponding with a type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="value">The new value.</param>
        public void Set<T>(T value)
        {
            KeyValue<T>.typeDictInstances[handle] = value;
            // Cache this value to be killed.
            ondeath.Add(RemoveVoid<T>);
        }

        /// <summary>
        /// Removes all types.
        /// </summary>
        public void Clear()
        {
            foreach (var item in ondeath)
            {
                item.Invoke();
            }
            ondeath = new HashSet<Action>();
        }

        // Keep this method non-inlined. We use its pointer as a hashcode for ondeath.
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void RemoveVoid<T>() => KeyValue<T>.typeDictInstances.Remove(handle);

        /// <summary>
        /// Removes the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to remove.</typeparam>
        /// <returns>True if the value existed and was removed; false if nothing happened.</returns>
        public bool Remove<T>()
        {
            if (KeyValue<T>.typeDictInstances.Remove(handle))
            {
                ondeath.Remove(RemoveVoid<T>);
                return true;
            }
            return false;
        }

        object? IServiceProvider.GetService(Type serviceType)
        {
            // Work around compile-time constaints >:)
            var del = Delegate.CreateDelegate(typeof(TypeDict), typeof(TypeDict).GetMethod("Get", 1, Type.EmptyTypes)!.MakeGenericMethod(serviceType));
            return del.DynamicInvoke(this);
            // Fuck you compiler I win
        }

        ~TypeDict()
        {
            // Clear all the references to types associated with this dict. Do this BEFORE clearing the handle, we still need access to it.
            foreach (var item in ondeath)
            {
                item.Invoke();
            }
            // Free the handle.
            handle.Free();
        }

        private static class KeyValue<T>
        {
            public static Dictionary<GCHandle, T> typeDictInstances = new Dictionary<GCHandle, T>();
        }
    }
}
