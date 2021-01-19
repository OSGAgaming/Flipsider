using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

using Flipsider.Engine.Input;
using Flipsider.Weapons;

namespace Flipsider
{
    public class GreenSlime : NPC
    {
        public static Texture2D icon = TextureCache.GreenSlime;
        protected override void SetDefaults()
        {
            life = 100;
            maxLife = 100;
            width = 64;
            framewidth = width;
            maxHeight = 52;
            height = maxHeight;
            position = Main.player.position;
            texture = TextureCache.GreenSlime;
            hostile = true;
            Collides = true;
        }
        protected override void PreDraw()
        {
            Utils.DrawText(life.ToString(), Color.White, Center + new Vector2(0, height / 2 + 10));
        }
        protected override void AI()
        {
            Constraints();
            Jump(2f);
            Animate(5, 1, 52, 0);
        }

    }
}
