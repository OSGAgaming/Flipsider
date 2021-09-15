namespace TextureCacheGenerator.Generator.Components.TypeHandling
{
    /// <summary>
    ///     Component for defining a class.
    /// </summary>
    public class ClassComponent : TokenPossessiveComponent
    {
        public string ClassName { get; }

        public ClassComponent(string className, Token tokens) : base(tokens)
        {
            ClassName = className;
        }

        public override string SerializeComponent() => $"{SerializeTokens()}class {ClassName} {{";
    }
}