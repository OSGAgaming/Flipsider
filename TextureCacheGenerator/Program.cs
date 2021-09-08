using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TextureCacheGenerator
{
    class Program
    {
        internal readonly static List<string> AssetPaths = new List<string>();
        public static string TextureCachePath => MainDirectory;
        public static void GenerateTextureCache()
        {
            FileStream stream = File.Open(TextureCachePath + @"\AutoloadTextureCache.cs", FileMode.Create);
            using (StreamWriter sw = new StreamWriter(stream))
            {
                sw.WriteLine($@"using Microsoft.Xna.Framework;");
                sw.WriteLine($@"using Microsoft.Xna.Framework.Content;");
                sw.WriteLine($@"using Microsoft.Xna.Framework.Graphics;");
                sw.WriteLine($@"using System.Collections.Generic;");
                sw.WriteLine($@"using System.Diagnostics;");
                sw.WriteLine($@"using System.IO;");

                sw.WriteLine($@"namespace Flipsider");
                sw.WriteLine(@"{");

                sw.WriteLine(@"   public static class Textures");
                sw.WriteLine(@"   {");

                sw.WriteLine(@"#nullable disable");

                foreach (string s in AssetPaths)
                {
                    string NewString = s.Replace(@"\", "_");
                    sw.WriteLine(@$"     public static Texture2D _{NewString};");
                }

                sw.WriteLine(@"     public static void LoadTextures()");
                sw.WriteLine(@"     {");
                foreach (string s in AssetPaths)
                {
                    string NewString = s.Replace(@"\", "_");
                    sw.WriteLine($"       _{NewString} = AutoloadTextures.Assets[@\"{s}\"];");
                }
                sw.WriteLine(@"     }");

                sw.WriteLine(@"   }");

                sw.WriteLine(@"}");
            }

        }
        public static void AddAssetsFromDirectories(string DirectoryPath)
        {
            if (Path.GetFileName(DirectoryPath) == "bin") return;

            string[] filePaths = Directory.GetFiles(DirectoryPath);
            for (int i = 0; i < filePaths.Length; i++)
            {
                string filePath = filePaths[i];

                if (!filePath.Contains(".png")) continue;

                string charSeprator = @"Textures\";
                int Index = filePath.IndexOf(charSeprator) + charSeprator.Length;
                string AlteredPath = filePath.Substring(Index);

                var AssetName = AlteredPath.Split(".pn")[0];

                AssetPaths.Add(AssetName);

                //Console.Write(AssetName + " Was Written!" + "\n");
            }
        }

        public static void GetAllAssetPaths(string DirectoryPath)
        {
            AddAssetsFromDirectories(DirectoryPath);

            string[] remainingDirecotries = Directory.GetDirectories(DirectoryPath);

            for (int i = 0; i < remainingDirecotries.Length; i++)
            {
                var DirectorySubPath = remainingDirecotries[i];

                if (Path.GetFileName(DirectorySubPath) == "bin") continue;

                //Console.Write("Loading Assetes From: [" + Path.GetFileName(DirectorySubPath) + "]\n");
                GetAllAssetPaths(DirectorySubPath);
                //Console.Write("\n\n");
            }
        }
        public static string AssetDirectory => Environment.ExpandEnvironmentVariables($@"{Environment.CurrentDirectory}\Content\Textures");
        public static string MainDirectory => Environment.ExpandEnvironmentVariables($@"{Environment.CurrentDirectory}\Content");
        static void Main(string[] args)
        {
            GetAllAssetPaths(AssetDirectory);
            GenerateTextureCache();
            //Console.WriteLine("A Total Of: " + AssetPaths.Count + " Assets were Written!");
            //Debug.Write(Environment.CurrentDirectory);
            //Console.ReadLine();
        }
    }
}
