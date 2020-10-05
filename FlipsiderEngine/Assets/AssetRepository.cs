using System.Collections.Generic;

namespace Flipsider.Assets
{
    public abstract class AssetRepository<T> where T : class
    {
        public abstract Asset<T>? TryFind(string name);

        public abstract void Unload();

        public Asset<T> FromThis(T value, string name)
        {
            var asset = new Asset<T>(value, name, this);
            added.AddLast(asset);
            return asset;
        }

        public IEnumerable<Asset<T>> AddedAssets => added;
        private LinkedList<Asset<T>> added = new LinkedList<Asset<T>>();
    }
}
