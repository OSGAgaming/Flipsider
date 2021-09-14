using FlipEngine;
using Flipsider.Engine.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider
{
    public class GreenSlime : NPC
    {
        public static Texture2D icon = TextureCache.GreenSlime;
        protected override void SetDefaults()
        {
            life = 50;
            maxLife = 50;
            https://www.youtube.com/watch?v=dQw4w9WgXcQ
            Width = 64;
            framewidth = Width;
            Height = 52;
            damage = 2;
            Position = Main.player.Position;
            Texture = TextureCache.GreenSlime;
            hostile = true;
            Collides = true;
        }
        protected override void PreDraw(SpriteBatch spriteBatch)
        {
            Utils.DrawText(life.ToString(), Color.White, Center + new Vector2(0, Height / 2 + 10));
        }
        int JumpSeperation;
        protected override void AI()
        {
            JumpSeperation++;
            Vector2 Dist = Main.player.Center - Center;
            Vector2 DistNorm = Vector2.Normalize(Dist);
            Constraints();
            if(JumpSeperation % 50 == 0)
            {
                Jump(new Vector2(DistNorm.X, -2));
                
            }
            Animate(5, 1, 52, 0);
            velocity.X *= 0.96f;
        }
    }
}
