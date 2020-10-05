using Flipsider.Core.Collections;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Flipsider.Assets.Repositories
{
    public class AsyncManifestRepo<T> : AsyncAssetRepository<T> where T : class
    {
        public AsyncManifestRepo(Assembly manifest, string root, T placeholder)
        {
            content = new ManifestContentManager(manifest, root);
            this.root = root;
            this.manifest = manifest;
            Placeholder = placeholder;
        }

        protected override T Placeholder { get; }

        private readonly string root;
        private readonly Assembly manifest;
        private readonly ManifestContentManager content;

        protected override bool CanLoad(string name)
        {
            string path = Path.ChangeExtension(Path.Combine(root, name), ".xnb");
            return manifest.GetManifestResourceInfo(path) != null;
        }

        protected override Task<T> GetValue(string name)
        {
            string path = Path.ChangeExtension(Path.Combine(root, name), ".xnb");
            return Task.Run(() => content.ReadAsset<T>(path));
        }

        public override void Unload()
        {
            content.Unload();
        }
    }

    internal class ManifestContentManager : ContentManager
    {
        private readonly Assembly manifest;

        public ManifestContentManager(Assembly manifest, string root) : base(FlipsiderGame.GameServices, root)
        {
            this.manifest = manifest;
        }

        protected override Stream OpenStream(string assetName)
        {
            return manifest.GetManifestResourceStream(assetName) ?? throw new ArgumentException("No manifest resource was found with the name '" + assetName + "'.");
        }

        public T ReadAsset<T>(string assetName)
        {
            return ReadAsset<T>(assetName, null);
        }
    }
}
