using FlipEngine;
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
            Width = 145;
            framewidth = Width;
            Height = 185;
            Position = Main.player.Position;
            Texture = TextureCache.Blob;
            hostile = true;
        }

        protected override void AI()
        {
            Constraints();
            //Jump(2f);
            Animate(5, 1, 185, 0);
            //Position.X += NormDistance.X;
        }
    }
}
