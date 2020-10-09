using System.Collections.ObjectModel;

namespace Flipsider.Localization
{
    /// <summary>
    /// Represents a localized string.
    /// </summary>
    public sealed class LocalizedText
    {
        private readonly bool literal;

        private LocalizedText(string key, bool literal, params object[] parameters)
        {
            Key = key;
            this.literal = literal;
            Parameters = new ReadOnlyCollection<object>(parameters);
        }

        public string Key { get; }
        public ReadOnlyCollection<object> Parameters { get; }

        public override string ToString()
        {
            return literal ? string.Format(Key, Parameters) : Language.Current.Get(Key, Parameters);
        }

        public static LocalizedText FromLiteral(string literal, params object[] parameters) => new LocalizedText(literal, true, parameters);
        public static LocalizedText FromKey(string literal, params object[] parameters) => new LocalizedText(literal, false, parameters);
    }
}
