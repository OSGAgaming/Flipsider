namespace FlipEngine
{
    public interface ILayeredComponent : IDrawable
    {
        public int Layer { get; set; }
    }
    public interface ILayeredComponentActive : ILayeredComponent
    {
        public bool InFrame { get; set; }
    }
    public interface IDrawData : ILayeredComponentActive
    {
        public DrawData drawData { get; set; }
    }
}
