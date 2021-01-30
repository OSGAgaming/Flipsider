using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Flipsider
{
    public class Bloom : LightSource
    {
        Entity BindableEntity;
        Texture2D BloomMap;
        public Bloom(Entity entity, Texture2D BloomMap, float str, Vector2 pos = default, Color col = default) : base(str, pos, col)
        {
            BindableEntity = entity;
            this.BloomMap = BloomMap;
            Main.AutoAppendToLayer(this);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Debug.Write("light");
            spriteBatch.End();
            Utils.BeginCameraSpritebatch();
            if(Lighting.Bloom != null)
            Utils.QuickApplyShader(Lighting.Bloom);
            spriteBatch.Draw(BloomMap, Main.player.Center - new Vector2(0, 18), Main.player.frame, Color.White, 0f, Main.player.frame.Size.ToVector2() / 2, 5f, Main.player.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            spriteBatch.End();
            Utils.BeginCameraSpritebatch();
        }
    }
}