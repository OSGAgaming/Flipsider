using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TextureCacheGenerator.Generator;
using TextureCacheGenerator.Generator.Components;
using TextureCacheGenerator.Generator.Components.Brackets;
using TextureCacheGenerator.Generator.Components.Directives;
using TextureCacheGenerator.Generator.Components.Methods;
using TextureCacheGenerator.Generator.Components.Namespaces;
using TextureCacheGenerator.Generator.Components.TypeHandling;

namespace TextureCacheGenerator.Flipsider
{
    public class CacheCodeGenerator : CodeGenerator
    {
        public static readonly List<string> Namespaces = new List<string>
        {
            "Microsoft.Xna.Framework",
            "Microsoft.Xna.Framework.Content",
            "Microsoft.Xna.Framework.Graphics",
            "System.Collections.Generic",
            "System.IO",
            "FlipEngine"
        };

        public List<string> CacheList { get; }

        public override List<ICodeComponent> Components { get; }

        public CacheCodeGenerator(List<string> cacheList)
        {
            CacheList = cacheList;

            Components = GetComponents().ToList();
        }

        public IEnumerable<ICodeComponent> GetComponents()
        {
            yield return new RawTextComponent("#nullable disable");
            yield return new RawTextComponent("// Auto-generated code.");
            yield return new RawTextComponent("// Generated using TextureCacheGenerator.");

            foreach (string ns in Namespaces)
                yield return new UsingDirectiveComponent(ns);

            yield return new NamespaceComponent("Flipsider");
            {

                yield return new ClassComponent("Textures", Token.Public | Token.Static);
                {

                    foreach (string newAsset in CacheList.Select(cachedAsset =>
                        cachedAsset.Replace(Path.DirectorySeparatorChar, '_')))
                        yield return new FieldComponent($"_{newAsset}", "Texture2D", Token.Public | Token.Static);

                    yield return new MethodComponent("LoadTextures", "void", new (string, string)[] { },
                        GetMethodContents(), Token.Public | Token.Static);

                }
                yield return new BracketComponent(BracketType.CurlyBrace, BracketDirection.Closing);

            }
            yield return new BracketComponent(BracketType.CurlyBrace, BracketDirection.Closing);
        }

        public string GetMethodContents()
        {
            StringBuilder builder = new StringBuilder();

            foreach (string asset in CacheList)
                builder.AppendLine($"_{asset.Replace(Path.DirectorySeparatorChar, '_')} = AutoloadTextures.Assets[@\"{asset}\"];");

            return builder.ToString();
        }
    }
}