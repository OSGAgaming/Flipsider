using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Weapons.Ranged.Pistol
{
    internal class TestGun2 : RangedWeapon
    {
        public TestGun2() : base(5, 5, 1)
        {
            SetInventoryIcon(TextureCache.testGun);
            reloadTime = 180;
            maxAmmo = 20;
        }

        protected override void OnActivate()
        {
            Main.Gamecamera.ScreenShake += 10;
        }

        public override void DrawInventory(SpriteBatch spriteBatch, Rectangle dest)
        {
            base.DrawInventory(spriteBatch, dest);

            Texture2D tex = TextureCache.magicPixel;
            Rectangle target = new Rectangle((int)dest.X, (int)dest.Y + 54, (int)(ammo / (float)maxAmmo * 48), 2);
            Rectangle targetUnder = new Rectangle((int)dest.X - 2, (int)dest.Y + 52, 52, 6);
            Color color = Color.Lerp(Color.Red, Color.LimeGreen, ammo / (float)(maxAmmo + 1));

            spriteBatch.Draw(tex, targetUnder, Color.Black);
            spriteBatch.Draw(tex, target, color);

            if (reloading)
            {
                Rectangle target2 = new Rectangle((int)dest.X, (int)dest.Y, 48, (int)(reload / (float)reloadTime * 48));
                spriteBatch.Draw(tex, target2, Color.Black * 0.5f);
            }
        }
    }
}
