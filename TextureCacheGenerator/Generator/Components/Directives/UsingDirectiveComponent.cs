namespace TextureCacheGenerator.Generator.Components.Directives
{
    /// <summary>
    ///     Component for adding a using directive.
    /// </summary>
    public class UsingDirectiveComponent : ICodeComponent
    {
        public string Namespace { get; }

        public UsingDirectiveComponent(string ns)
        {
            Namespace = ns;
        }

        public string SerializeComponent() => $"using {Namespace};";
    }
}