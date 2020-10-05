using System.IO;
using System.Reflection;

namespace Flipsider.Assets.Repositories
{
    public class ManifestRepo<T> : AssetRepository<T> where T : class
    {
        public ManifestRepo(Assembly manifest, string root)
        {
            content = new ManifestContentManager(manifest, root);
            this.manifest = manifest;
            this.root = root;
        }

        private readonly string root;
        private readonly Assembly manifest;
        private readonly ManifestContentManager content;

        public override Asset<T>? TryFind(string name)
        {
            string path = Path.ChangeExtension(Path.Combine(root, name), ".xnb");
            if (manifest.GetManifestResourceInfo(path) != null)
            {
                return FromThis(content.ReadAsset<T>(path), name);
            }
            return null;
        }

        public override void Unload()
        {
            content.Unload();
        }
    }
}
