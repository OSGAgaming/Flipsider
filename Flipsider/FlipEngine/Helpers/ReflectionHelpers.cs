using System;
using System.Collections.Generic;
using System.Linq;

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
    } 
}
