using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

using Flipsider.Engine.Input;
using Flipsider.Weapons;

namespace Flipsider
{
    public class Projectile : Entity
    {
        public bool pickable;
        public bool hostile;
        public float alpha;
        public float rotation;

        public static void SpawnProjectile()
        {

        }
        public static void SpawnProjectile<T>(Vector2 position) where T : Projectile, new()
        {
            T Projectile = new T();
            Projectile.SetDefaults();
            Projectile.position = position;
        }

        public static void ShootProjectileAtCursor<T>(Vector2 position, float vel) where T : Projectile, new()
        {
            T Projectile = new T();
            Projectile.SetDefaults();
            Projectile.position = position;
            Projectile.velocity = Vector2.Normalize(Main.MouseScreen.ToVector2() - Main.player.Center) * vel;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Center, frame, Color.White, 0f, frame.Size.ToVector2() / 2, 1f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            DrawMethods.DrawRectangle(position + new Vector2(0, maxHeight - height), width, height, Color.Green);
        }
        protected virtual void SetDefaults()
        {

        }
    }
}
