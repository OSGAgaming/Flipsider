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
        protected override void SetDefaults()
        {
            life = 100;
            maxLife = 100;
            width = 185;
            height = 165;
            maxHeight = 50;
            position = Main.player.position;
            texture = TextureCache.Blob;
        }

        protected override void AI()
        {

        }

    }
}
