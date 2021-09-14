using System.Collections.Generic;
using System.Text;
using TextureCacheGenerator.Generator.Components;

namespace TextureCacheGenerator.Generator
{
    /// <summary>
    ///     Code generator, holds a collection of components.
    /// </summary>
    public class CodeGenerator
    {
        /// <summary>
        ///     A collection of components to be serialized to the file.
        /// </summary>
        public virtual List<ICodeComponent> Components { get; }

        public CodeGenerator(List<ICodeComponent> components = null)
        {
            Components = components ?? new List<ICodeComponent>();
        }

        /// <summary>
        ///     Generate a string containing all of the serialized <see cref="Components"/>.
        /// </summary>
        /// <returns></returns>
        public virtual string GenerateCode()
        {
            StringBuilder builder = new StringBuilder();

            foreach (ICodeComponent component in Components)
                builder.AppendLine(component.SerializeComponent());

            return builder.ToString();
        }
    }
}