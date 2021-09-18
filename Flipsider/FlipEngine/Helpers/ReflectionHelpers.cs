using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlipEngine
{
    public static partial class Utils
    {
        public static Type[] GetInheritedClasses(Type MyType)
        {
            return MyType.Assembly.GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && MyType.IsAssignableFrom(TheType)).ToArray();
        }

        public static IEnumerable<T> GetInheritedClasses<T>()
        {
            foreach (Type instance in GetInheritedClasses(typeof(T)))
            {
                T Screen = (T)Activator.CreateInstance(instance);
                if(Screen != null) yield return Screen;
            }
        }

        public static Type? GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                     "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }
    } 
}
