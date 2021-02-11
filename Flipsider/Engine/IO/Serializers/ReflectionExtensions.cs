using System;
using System.Reflection;

namespace Flipsider.Engine
{
    public static class ReflectionExtensions
    {
        public static Type[] GetTypesSafe(this Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types ?? throw new Exception(""); ;
            }
        }

        public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method) where TDelegate : Delegate => (TDelegate)method.CreateDelegate(typeof(TDelegate));

        public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method, object target) where TDelegate : Delegate => (TDelegate)method.CreateDelegate(typeof(TDelegate), target);

        public static bool IsStruct(this Type type) => type.IsValueType || Nullable.GetUnderlyingType(type) != null;

        public static bool IsNullable(this Type type) => !(type.IsValueType && Nullable.GetUnderlyingType(type) == null);

        public static bool IsSubclassOfGeneric(this Type type, Type generictype)
        {
            while (type != null && type != typeof(object))
            {
                if ((type.IsGenericType ? type.GetGenericTypeDefinition() : type) == generictype)
                {
                    return true;
                }

                type = type.BaseType ?? throw new Exception("");
            }
            return false;
        }

        public static bool IsSubclassOfGeneric(this Type type, Type generictype, out Type? gType)
        {
            gType = type;
            while (gType != null && gType != typeof(object))
            {
                if ((gType.IsGenericType ? gType.GetGenericTypeDefinition() : gType) == generictype)
                {
                    return true;
                }

                gType = gType.BaseType ?? throw new Exception("");
            }
            gType = null;
            return false;
        }

        public static bool ImplementsInterface(this Type type, Type interfaceType) => interfaceType.IsAssignableFrom(type);

        public static bool ImplementsInterface<T>(this Type type) where T : class => typeof(T).IsAssignableFrom(type);

#pragma warning disable CS8601 // Possible null reference assignment.
        public static bool TryCreateInstance(this Type type, out object result) => TryCreateInstance(type, false, out result);
#pragma warning restore CS8601 // Possible null reference assignment.

        public static bool TryCreateInstance(this Type type, bool privateConstructor, out object? result)
        {
            if (CouldBeInstantiated(type) && DefaultConstructor(type, privateConstructor) != null)
            {
                result = Activator.CreateInstance(type);
                return true;
            }
            result = null;
            return false;
        }

        public static bool TryCreateInstance<T>(this Type type, out T result) => TryCreateInstance(type, false, out result);

        public static bool TryCreateInstance<T>(this Type type, bool privateConstructor, out T result)
        {
            if (CouldBeInstantiated(type) && DefaultConstructor(type, privateConstructor) != null)
            {
                result = (T)Activator.CreateInstance(type) ?? throw new Exception(""); ;
                return true;
            }

#pragma warning disable CS8601 // Possible null reference assignment.
            result = default;
#pragma warning restore CS8601 // Possible null reference assignment.
            return result != null;
        }

        public static ConstructorInfo? DefaultConstructor(this Type type, bool nonPublic = false) => nonPublic ? type.GetConstructor(FlagsInstance, null, Type.EmptyTypes, null) : type.GetConstructor(Type.EmptyTypes);

        public const BindingFlags FlagsInstance = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static bool CouldBeInstantiated(this Type type) => type.IsValueType || !type.IsAbstract && (type.IsGenericType == type.IsConstructedGenericType);
    }
}