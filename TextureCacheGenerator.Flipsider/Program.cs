using System;
using System.Collections.Generic;
using System.IO;

namespace TextureCacheGenerator.Flipsider
{
    public static class Program
    {
        public static readonly List<string> AssetPaths = new List<string>();

        public static string AssetDirectory =>
            Environment.ExpandEnvironmentVariables($@"{Environment.CurrentDirectory}\Content\Textures");

        public static string MainDirectory =>
            Environment.ExpandEnvironmentVariables($@"{Environment.CurrentDirectory}\Content");

        public static void GetAllAssetPaths(string directoryPath)
        {
            AddAssetsFromDirectories(directoryPath);

            string[] remainingDirectories = Directory.GetDirectories(directoryPath);

            foreach (string directorySubPath in remainingDirectories)
            {
                if (Path.GetFileName(directorySubPath) == "bin")
                    continue;

                // Console.Write("Loading Assets From: [" + Path.GetFileName(DirectorySubPath) + "]\n");

                GetAllAssetPaths(directorySubPath);
            }
        }

        public static void AddAssetsFromDirectories(string directoryPath)
        {
            if (Path.GetFileName(directoryPath) == "bin")
                return;

            string[] filePaths = Directory.GetFiles(directoryPath);

            foreach (string filePath in filePaths)
            {
                if (!filePath.Contains(".png")) 
                    continue;

                const string charSeparator = @"Textures\";
                int index = filePath.IndexOf(charSeparator, StringComparison.Ordinal) + charSeparator.Length;
                string alteredPath = filePath[index..];

                string assetName = alteredPath.Split(".pn")[0];

                AssetPaths.Add(assetName);
            }
        }

        public static void GenerateTextureCache()
        {
            string path = Path.Combine(MainDirectory, "AutoloadTextureCache.cs");
            using FileStream stream = File.Open(path, FileMode.Create);
            using StreamWriter writer = new StreamWriter(stream);
            writer.Write(new CacheCodeGenerator(AssetPaths).GenerateCode());
        }

        public static void Main()
        {
            GetAllAssetPaths(AssetDirectory);
            GenerateTextureCache();
        }
    }
}