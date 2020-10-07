using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text.Json;

namespace Flipsider.Localization
{
    /// <summary>
    /// Represents a localized string.
    /// </summary>
    public class LocalizedText
    {
        public LocalizedText(string key, params object[] parameters)
        {
            Key = key;
            Parameters = new ReadOnlyCollection<object>(parameters);
        }

        public string Key { get; }
        public ReadOnlyCollection<object> Parameters { get; }

        public override string ToString()
        {
            return Language.Current.Get(Key, Parameters);
        }

        public static implicit operator LocalizedText(string key)
        {
            return new LocalizedText(key);
        }
    }

    /// <summary>
    /// Represents a language with keys and values specified in a JSON document.
    /// </summary>
    public class Language
    {
        private Language(JsonDocument file)
        {
            this.file = file;
        }

        private readonly Dictionary<string, string> cachedKeys = new Dictionary<string, string>(StringComparer.Ordinal);
        private readonly JsonDocument file;
        private static readonly Dictionary<IsoCode, Language> cachedLanguages = new Dictionary<IsoCode, Language>();

        /// <summary>
        /// The current language in use.
        /// </summary>
        public static Language Current { get; set; } = From(IsoCode.English);

        /// <summary>
        /// Gets a language from its relevant <see cref="IsoCode"/>.
        /// </summary>
        public static Language From(IsoCode languageCode)
        {
            if (cachedLanguages.TryGetValue(languageCode, out var language))
            {
                return language;
            }

            RegisterLanguage(languageCode, typeof(Language).Assembly, "Localization/LanguageKeys/");
            
            return cachedLanguages[languageCode];
        }

        /// <summary>
        /// Registers a language given its <see cref="IsoCode"/> and JSON contents.
        /// </summary>
        public static void RegisterLanguage(IsoCode languageCode, JsonDocument json)
        {
            cachedLanguages.Add(languageCode, new Language(json));
        }

        /// <summary>
        /// Registers a language given its <see cref="IsoCode"/> and a root directory within an assembly manifest. <para/>
        /// The assembly should embed the JSON file as "<paramref name="languageFileRootDirectory"/>/{iso}.json" where {iso} is the ISO code of the language. <para/>
        /// Comments and trailing comments are ignored automatically in the JSON document.
        /// </summary>
        public static void RegisterLanguage(IsoCode languageCode, Assembly assembly, string languageFileRootDirectory)
        {
            using var stream = assembly.GetManifestResourceStream(Path.Combine(languageFileRootDirectory, languageCode.ToString()));
            if (stream == null)
                throw new InvalidOperationException("No language file with that ISO 639-1 code was found. Visit https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes for a list of valid language ISO codes.");
            var doc = JsonDocument.Parse(stream, new JsonDocumentOptions { AllowTrailingCommas = true, CommentHandling = JsonCommentHandling.Skip });

            cachedLanguages.Add(languageCode, new Language(doc));
        }

        /// <summary>
        /// Gets a string value from the specified (case-sensitive) JSON string or throws if it isn't found.
        /// <para/> For example: "Hi" gets the root->"Hi" property's value; "NPC.Hi" gets the root->"NPC"->"Hi" property's "Hi" property's value.
        /// </summary>
        public string Get(string key)
        {
            if (cachedKeys.TryGetValue(key, out var value))
                return value;

            var properties = key.Split('.');
            var element = file.RootElement;

            foreach (var item in properties)
            {
                if (!element.TryGetProperty(item, out element))
                {
                    throw new FormatException("Expected a JSON property, got: " + element.ValueKind + ".");
                }
            }

            try
            {
                return element.GetString();
            }
            catch
            {
                throw new FormatException("Expected a JSON string, got: " + element.ValueKind + ".");
            }
        }

        /// <summary>
        /// Calls <see cref="Get(string)"/> and runs it through <see cref="string.Format(string, object?[])"/>.
        /// </summary>
        public string Get(string key, params object[] parameters)
        {
            return string.Format(Get(key), parameters);
        }

        /// <summary>
        /// Equivalent to <see cref="Get(string)"/>.
        /// </summary>
        public string this[string key] => Get(key);
        /// <summary>
        /// Equivalent to <see cref="Get(string, object[])"/>
        /// </summary>
        public string this[string key, params object[] parameters] => Get(key, parameters);
    }

    /// <summary>
    /// Represents an ISO 639-1 language code.
    /// </summary>
    public struct IsoCode
    {
        private IsoCode(char first, char second)
        {
            First = char.ToLower(first);
            Second = char.ToLower(second);
        }

        public static IsoCode English { get; } = "en";

        public char First { get; }
        public char Second { get; }

        public override bool Equals(object? obj)
        {
            return obj is IsoCode code &&
                   First == code.First &&
                   Second == code.Second;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(First, Second);
        }

        public static bool operator ==(IsoCode left, IsoCode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IsoCode left, IsoCode right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return First.ToString() + Second.ToString();
        }

        public static implicit operator IsoCode(string value)
        {
            if (value.Length != 2)
            {
                throw new InvalidOperationException("ISO 639-1 language code must be 2 characters.");
            }
            return new IsoCode(value[0], value[1]);
        }
    }
}
