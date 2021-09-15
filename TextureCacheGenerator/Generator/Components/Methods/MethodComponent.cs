using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextureCacheGenerator.Generator.Components.Methods
{
    // this sucks
    /// <summary>
    ///     Component for adding a method.
    /// </summary>
    public class MethodComponent : TokenPossessiveComponent
    {
        public string MethodName { get; }

        public IEnumerable<(string type, string parameter)> Parameters { get; }

        public string MethodContents { get; }

        public string ReturnType { get; }

        public MethodComponent(string methodName, string returnType,
            IEnumerable<(string type, string parameter)> parameters, string methodContents, Token tokens) : base(tokens)
        {
            MethodName = methodName;
            Parameters = parameters;
            MethodContents = methodContents;
            ReturnType = returnType;
            _ = typeof(void);
        }

        public MethodComponent(string methodName, Type returnType,
            IEnumerable<(Type type, string parameter)> parameters, string methodContents, Token tokens) : base(tokens)
        {
            MethodName = methodName;
            Parameters = parameters.Select(x => (x.type.FullName, x.parameter));
            MethodContents = methodContents;
            ReturnType = returnType.FullName;
        }

        public override string SerializeComponent()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"{SerializeTokens()}{ReturnType} {MethodName}(");

            bool isFirst = true;
            foreach ((string type, string parameter) in Parameters)
            {
                if (!isFirst)
                    builder.Append(',');

                builder.Append($"{type} {parameter}");

                isFirst = false;
            }

            builder.AppendLine(") {");
            builder.AppendLine(MethodContents);
            builder.Append('}');

            return builder.ToString();
        }
    }
}