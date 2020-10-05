using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

using Flipsider.Engine.Input;
using Flipsider.Weapons;
using Flipsider.Engine.Assets;

namespace Flipsider
{
    public class GreenSlime : NPC
    {
        public static Texture2D icon = AssetManager.GreenSlime;
        protected override void SetDefaults()
        {
            life = 100;
            maxLife = 100;
            width = 64;
            framewidth = width;
            maxHeight = 52;
            height = maxHeight;
            position = Main.player.position;
            texture = AssetManager.GreenSlime;
            Collides = true;
        }

        protected override void AI()
        {
            Jump(2f);
            Animate(5, 1, 52, 0);
        }

    }
}
