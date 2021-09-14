using System.IO;

namespace Flipsider.Engine.Interfaces
{
    public interface ISerializable<T>
    {
        public void Serialize(Stream stream);
        public T Deserialize(Stream stream);
    }
}
