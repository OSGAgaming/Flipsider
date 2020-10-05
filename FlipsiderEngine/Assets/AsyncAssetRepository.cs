using Flipsider.Assets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flipsider.Assets
{
    public abstract class AsyncAssetRepository<T> : AssetRepository<T> where T : class
    {
        public sealed override Asset<T>? TryFind(string name)
        {
            if (CanLoad(name))
            {
                return new AsyncAsset(Placeholder, name, this);
            }
            return null;
        }

        protected abstract T Placeholder { get; }
        protected abstract Task<T> GetValue(string name);
        protected abstract bool CanLoad(string name);

        private class AsyncAsset : Asset<T>
        {
            internal AsyncAsset(T val, string name, AsyncAssetRepository<T> sourceRepo) : base(val, name, sourceRepo)
            {
                sourceRepo.GetValue(name).ContinueWith(CompleteLoad);
            }

            void CompleteLoad(Task<T> task)
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Value = task.Result;
                }
                if (task.Exception != null)
                {
                    System.Diagnostics.Debug.WriteLine(task.Exception);
                }
            }
        }
    }
}
