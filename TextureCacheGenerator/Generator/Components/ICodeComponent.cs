namespace TextureCacheGenerator.Generator.Components
{
    /// <summary>
    ///     Code component interface for code generation.
    /// </summary>
    public interface ICodeComponent
    {
        /// <summary>
        ///     Serialize the component as a string.
        /// </summary>
        /// <returns>The serialized component as it should appear in the code.</returns>
        string SerializeComponent();
    }
}