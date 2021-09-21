using Microsoft.Xna.Framework.Graphics;

namespace FlipEngine
{
    public interface ILayeredComponent : IDrawable
    {
        public int Layer { get; set; }
    }
    public interface ILayeredComponentActive : ILayeredComponent
    {
        public bool InFrame { get; }
    }
    public interface IDrawData : ILayeredComponentActive
    {
        public DrawData drawData { get; set; }
    }

    public interface IPrimitiveLayeredComponent
    {
        public void DrawPrimtiivesBefore(SpriteBatch sb);
        public void DrawPrimtiivesAfter(SpriteBatch sb);
        public int Layer { get; set; }

    }
}
