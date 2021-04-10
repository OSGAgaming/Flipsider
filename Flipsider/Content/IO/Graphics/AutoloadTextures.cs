using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Flipsider
{
    // TODO dude.
#nullable disable
    public static class AutoloadTextures
    {
        internal readonly static List<string> AssetPaths = new List<string>();

        internal static Dictionary<string, Texture2D> Assets = new Dictionary<string, Texture2D>();
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

                Debug.Write(AssetName + "\n");
            }
        }

        public static void GetAllAssetPaths(string DirectoryPath)
        {
            AddAssetsFromDirectories(DirectoryPath);

            string[] remainingDirecotries = Directory.GetDirectories(DirectoryPath);

            for(int i = 0; i< remainingDirecotries.Length; i++)
            {
                var DirectorySubPath = remainingDirecotries[i];

                if (Path.GetFileName(DirectorySubPath) == "bin") continue;

                Debug.Write("Loading Assetes From: [" + Path.GetFileName(DirectorySubPath) + "]\n");
                GetAllAssetPaths(DirectorySubPath);
                Debug.Write("\n\n");
            }
        }

        public static void LoadTexturesToAssetCache(ContentManager content)
        {
            foreach(string Asset in AssetPaths)
            {
                Assets.Add(Asset, content.Load<Texture2D>(Path.GetFileName(Utils.AssetDirectory) + $"/" + Asset));
            }
        }
    }
}
