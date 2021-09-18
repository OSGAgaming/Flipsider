using System;
using System.Linq;
using System.Reflection;

namespace Flipsider
{
    public static partial class Utils
    {
        public static Type[] GetInheritedClasses(Type MyType)
        {
            return MyType.Assembly.GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && MyType.IsAssignableFrom(TheType)).ToArray();
        }


    }
}
