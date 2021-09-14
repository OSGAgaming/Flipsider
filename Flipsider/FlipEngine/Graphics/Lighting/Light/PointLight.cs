using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace FlipEngine
{
    public class PointLight : LightSource
    {
        public PointLight(float str, Vector2 pos, Color col) : base(str, pos, col)
        {

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}