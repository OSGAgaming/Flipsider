using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Flipsider.Weapons
{
    internal class ShortSword : Sword
    {
        public override Texture2D swordSheet => TextureCache.GreenSlime;

        public ShortSword() : base(5, 30, 3)
        {
            SetInventoryIcon(TextureCache.GreenSlime);
        }

        protected override void OnActivate()
        {
            Projectile.ShootProjectileAtCursor<ExampleProj>(Main.player.Center, 3f);
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
            damage = 50;
            width = 20;
            hostile = false;
            framewidth = width;
            height = 20;
            texture = TextureCache.magicPixel;
            Collides = true;
            noGravity = true;
            gravity = 0.1f;
            friction = 0.98f;
            noAirResistance = true;
            TileCollide = true;
        }

        protected override void OnAI()
        {
            Constraints();
            Animate(5, 1, 20, 0);
        }
    }
}
