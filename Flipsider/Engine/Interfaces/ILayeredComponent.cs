namespace Flipsider.Engine.Interfaces
{
    public interface ILayeredComponent : IDrawable
    {
        public int Layer { get; set; }
    }
}
