namespace Flipsider.Engine.Interfaces
{
    public interface ISerializable
    {
        public void Serialize(string path);
        public void Deserialze(string path);
    }
}
