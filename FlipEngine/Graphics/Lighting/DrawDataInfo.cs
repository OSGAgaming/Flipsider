using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace FlipEngine
{
    public class DrawDataInfo
    {
        public Texture2D? Texture;
        public Vector2 Position;
        public Vector2? Scale;
        public Rectangle? SourceRect;
        public Color Tint;
        public SpriteEffects Effects;
        public float Rotation;
        public Vector2 Origin;
    }
}