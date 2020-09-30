using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

using Flipsider.Engine.Input;
using Flipsider.Weapons;

namespace Flipsider
{
    public class Blob : NPC
    {
        public static Texture2D icon = TextureCache.Blob;
        protected override void SetDefaults()
        {
            life = 100;
            maxLife = 100;
            width = 145;
            framewidth = width;
            maxHeight = 185;
            height = maxHeight;
            position = Main.player.position;
            texture = TextureCache.Blob;
            Collides = true;
        }

        protected override void AI()
        {
            Jump(2f);
            Animate(5, 1, 185, 0);
        }
    }
}
