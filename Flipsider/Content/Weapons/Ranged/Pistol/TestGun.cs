﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Weapons.Ranged.Pistol
{
    internal class TestGun : RangedWeapon
    {
        public TestGun() : base(5, 30, 3)
        {
            SetInventoryIcon(TextureCache.testGun);
            reloadTime = 120;
            maxAmmo = 3;
        }

        protected override void OnActivate()
        {
            Camera.screenShake += 10;
        }

        public override void DrawInventory(SpriteBatch spriteBatch, Vector2 pos)
        {
            base.DrawInventory(spriteBatch, pos);

            Texture2D tex = TextureCache.magicPixel;
            Rectangle target = new Rectangle((int)pos.X, (int)pos.Y + 54, (int)(ammo / (float)maxAmmo * 48), 2);
            Rectangle targetUnder = new Rectangle((int)pos.X - 2, (int)pos.Y + 52, 52, 6);
            Color color = Color.Lerp(Color.Red, Color.LimeGreen, ammo / (float)(maxAmmo + 1));

            spriteBatch.Draw(tex, targetUnder, Color.Black);
            spriteBatch.Draw(tex, target, color);

            if (reloading)
            {
                Rectangle target2 = new Rectangle((int)pos.X, (int)pos.Y, 48, (int)(reload / (float)reloadTime * 48));
                spriteBatch.Draw(tex, target2, Color.Black * 0.5f);
            }
        }
    }

    internal class TestBullet : LivingEntity
    {
        public TestBullet()
        {
            texture = TextureCache.magicPixel;
            frame = new Rectangle(0, 0, 2, 2);
        }
    }
}
