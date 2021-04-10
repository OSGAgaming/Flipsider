using Microsoft.Xna.Framework.Graphics;

namespace Flipsider
{
    public class Blob : NPC
    {
        public static Texture2D icon = TextureCache.Blob;
        public static int Type = 0;
        protected override void SetDefaults()
        {
            life = 100;
            maxLife = 100;
            width = 145;
            framewidth = width;
            height = 185;
            position = Main.player.position;
            texture = TextureCache.Blob;
            hostile = true;
            Collides = true;
        }

        protected override void AI()
        {
            Constraints();
            //Jump(2f);
            Animate(5, 1, 185, 0);
            position.X += NormDistance.X;
        }
    }
}
