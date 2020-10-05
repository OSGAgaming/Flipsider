using Flipsider.Assets;
using Flipsider.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider
{
    public class DrawDataInfo
    {
        public DrawDataInfo(Asset<Texture2D> texture)
        {
            Texture = texture;
        }
        public Asset<Texture2D> Texture { get; }
        public Vector2 Position;
        public Vector2? Scale;
        public Rectangle? SourceRect;
        public Color Tint;
        public SpriteEffects Effects;
        public Rotation Rotation;
        public Vector2 Origin;
    }
}