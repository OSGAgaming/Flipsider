using System;
using System.Text;

namespace TextureCacheGenerator.Generator.Components
{
    // TODO: Add more?
    [Flags]
    public enum Token
    {
        None = 0,
        Public = 1,
        Protected = 2,
        Private = 4,
        Internal = 8,
        Partial = 16,
        Static = 32
    }

    public abstract class TokenPossessiveComponent : ICodeComponent
    {
        public Token Tokens { get; }

        protected TokenPossessiveComponent(Token tokens)
        {
            Tokens = tokens;
        }

        public virtual string SerializeTokens()
        {
            StringBuilder builder = new StringBuilder();

            void Add(Token token, string value)
            {
                if (Tokens.HasFlag(token))
                    builder.Append($"{value} "); // space at the end!
            }

            if (Tokens.HasFlag(Token.None))
                return "";

            Add(Token.Public, "public");
            Add(Token.Protected, "protected");
            Add(Token.Private, "private");
            Add(Token.Internal, "internal");
            Add(Token.Partial, "partial");
            Add(Token.Static, "static");

            return builder.ToString();
        }

        public abstract string SerializeComponent();
    }
}