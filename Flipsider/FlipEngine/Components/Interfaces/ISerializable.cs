using System.IO;

namespace FlipEngine
{
    public interface ISerializable<T>
    {
        public void Serialize(Stream stream);
        public T Deserialize(Stream stream);
    }
}
