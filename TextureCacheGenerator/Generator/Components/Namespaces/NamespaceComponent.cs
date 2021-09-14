namespace TextureCacheGenerator.Generator.Components.Namespaces
{
    /// <summary>
    ///     A component for adding namespaces.
    /// </summary>
    public class NamespaceComponent : ICodeComponent
    {
        /// <summary>
        ///     The namespace to serialize.
        /// </summary>
        public string Namespace { get; }

        public NamespaceComponent(string ns)
        {
            Namespace = ns;
        }

        public string SerializeComponent() => $"namespace {Namespace};";
    }
}