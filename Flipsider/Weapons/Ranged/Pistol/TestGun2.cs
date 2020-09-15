using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.Weapons.Ranged.Pistol
{
    class TestGun2 : RangedWeapon
    {
        public TestGun2() : base(5, 5, 1)
        {
            inventoryIcon = TextureCache.testGun;
            reloadTime = 180;
            maxAmmo = 20;
        }

        protected override void OnActivate()
        {
            Camera.screenShake += 2;
            inventoryIcon = TextureCache.testGun;
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
}
