using System;

namespace TextureCacheGenerator.Generator.Components.TypeHandling
{
    /// <summary>
    ///     Component for adding a field.
    /// </summary>
    public class FieldComponent : TokenPossessiveComponent
    {
        public string FieldName { get; }

        public string FieldType { get; }

        public FieldComponent(string fieldName, string fieldType, Token tokens) : base(tokens)
        {
            FieldName = fieldName;
            FieldType = fieldType;
        }

        public FieldComponent(string fieldName, Type fieldType, Token tokens) : this(fieldName, fieldType.FullName,
            tokens)
        {
        }

        public override string SerializeComponent() => $"{SerializeTokens()}{FieldType} {FieldName};";
    }

    /// <summary>
    ///     Component for adding a field of the given generic type.
    /// </summary>
    public class FieldComponent<T> : FieldComponent
    {
        public FieldComponent(string fieldName, Token tokens) : base(fieldName, typeof(T), tokens)
        {
        }
    }
}