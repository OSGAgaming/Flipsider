﻿using FMOD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Weapons
{
    class ShortSword : Sword
    {
        public override Texture2D swordSheet => TextureCache.GreenSlime;

        public ShortSword() : base(5, 30, 3)
        {
            SetInventoryIcon(TextureCache.GreenSlime);
        }

        protected override void OnActivate()
        {
            Projectile.ShootProjectileAtCursor<ExampleProj>(Main.player.Center,3f);
        }

        public override void DrawInventory(SpriteBatch spriteBatch, Vector2 pos)
        {
            base.DrawInventory(spriteBatch, pos);
        }
    }

    public class ExampleProj : Projectile
    {
        public static Texture2D icon = TextureCache.Blob;
        protected override void SetDefaults()
        {
            width = 20;
            maxHeight = 20;
            framewidth = width;
            height = maxHeight;
            texture = TextureCache.magicPixel;
            Collides = true;
            noGravity = true;
            noAirResistance = true;
        }

        protected override void AI()
        {
            Constraints();
            Animate(5, 1, 20, 0);
        }
    }
}