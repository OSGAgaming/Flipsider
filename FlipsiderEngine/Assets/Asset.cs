namespace Flipsider.Assets
{
    public class Asset<T> where T : class
    {
        public T Value { get; protected set; }
        public string Name { get; }
        public AssetRepository<T> SourceRepo { get; }

        internal Asset(T val, string name, AssetRepository<T> sourceRepo)
        {
            Value = val;
            Name = name;
            SourceRepo = sourceRepo;
        }

        public static Asset<T> From(string name)
        {
            return AssetManager<T>.Get(name);
        }

        public static implicit operator Asset<T>(string val)
        {
            return From(val);
        }

        public static implicit operator T(Asset<T> val)
        {
            return val.Value;
        }
    }
}
