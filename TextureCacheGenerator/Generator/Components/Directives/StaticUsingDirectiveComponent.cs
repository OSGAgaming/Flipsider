using System;

namespace TextureCacheGenerator.Generator.Components.Directives
{
    /// <summary>
    ///     Component for adding a static using directive.
    /// </summary>
    public class StaticUsingDirectiveComponent : ICodeComponent
    {
        public string TypeName { get; }

        public StaticUsingDirectiveComponent(string typeName)
        {
            TypeName = typeName;
        }

        public StaticUsingDirectiveComponent(Type type) : this(type.FullName)
        {
        }

        public string SerializeComponent() => $"using static {TypeName};";
    }
}