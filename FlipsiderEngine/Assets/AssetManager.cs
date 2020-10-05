using Flipsider.Core;
using Flipsider.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace Flipsider.Assets
{
    /// <summary>
    /// Manages all assets in the game.
    /// </summary>
    /// <typeparam name="T">The type of asset.</typeparam>
    public static class AssetManager<T> where T : class
    {
        static AssetManager()
        {
            AssetManager.clear += ClearRepos;
        }

        public static IEnumerable<AssetRepository<T>> AssetRepos => assetRepos;
        private static readonly HashSet<AssetRepository<T>> assetRepos = new HashSet<AssetRepository<T>>();
        private static readonly Dictionary<string, Asset<T>> assets = new Dictionary<string, Asset<T>>();

        /// <summary>
        /// Adds a repository for the specified asset.
        /// </summary>
        /// <param name="repo">The repository.</param>
        public static void AddRepo(AssetRepository<T> repo)
        {
            assetRepos.Add(repo);
        }

        /// <summary>
        /// Unloads a repository and all its assets and removes it from the list of repositories.
        /// </summary>
        public static void UnloadRepo(AssetRepository<T> repo)
        {
            // There may be a lot of textures that need removed, so do it slowly.
            repo.Unload();
            foreach (var item in repo.AddedAssets)
            {
                assets.Remove(item.Name);
            }
            assetRepos.Remove(repo);
        }

        /// <summary>
        /// Unloads all matching repositories and their matching assets.
        /// </summary>
        public static void UnloadReposWhere(Predicate<AssetRepository<T>> predicate)
        {
            foreach (var item in assetRepos)
            {
                if (predicate(item))
                    UnloadRepo(item);
            }
        }

        /// <summary>
        /// Clears all repositories for this resource.
        /// </summary>
        public static void ClearRepos()
        {
            foreach (var item in assetRepos)
            {
                item.Unload();
            }
            assetRepos.Clear();
            assets.Clear();
        }

        /// <summary>
        /// Gets an asset from one of the added repositories and caches it if needed.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The cached asset.</returns>
        public static Asset<T> Get(string name)
        {
            AssetManager.Clean(ref name);
            if (assets.TryGetValue(name, out var asset))
            {
                return asset;
            }
            foreach (var repo in AssetRepos)
            {
                asset = repo.TryFind(name);
                if (asset != null)
                {
                    return assets[name] = asset;
                }
            }
            throw new InvalidOperationException($"No matching asset for '{name}' found.");
        }
    }

    /// <summary>
    /// Manages all assets regardless of resource type.
    /// </summary>
    public static class AssetManager
    {
        /// <summary>
        /// Clears all repos irrevocably.
        /// </summary>
        public static void Clear() => clear?.Invoke();
        internal static Action? clear;

        /// <summary>
        /// Cleans a path up for repositories.
        /// </summary>
        /// <param name="path">The path to clean.</param>
        public static void Clean(ref string path)
        {
            path = path.Replace('\\', '/').Trim(' ', '/');
        }
    }
}
