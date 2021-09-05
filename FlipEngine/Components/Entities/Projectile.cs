using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlipEngine
{
    public class Projectile : LivingEntity
    {
        public bool pickable;
        public bool hostile;
        public float alpha;
        public float rotation;
        public bool TileCollide;
        public int damage;
        public bool EntityCollide()
        {
            foreach (Entity entity in Chunk.Entities)
            {
                if (entity is NPC)
                {
                        if ((entity as NPC)?.hostile == !hostile)
                        {
                            if (entity.CollisionFrame.Intersects(CollisionFrame))
                            {
                                (entity as NPC)?.TakeDamage(damage);
                                return true;
                            }
                        }
                }
            }
            return false;
        }
        protected virtual void OnAI()
        {

        }
        protected override void AI()
        {
            EntityCollide();
            OnAI();
        }
        public static void SpawnProjectile()
        {

        }
        protected override void OnCollide()
        {
            if (TileCollide)
            {
                Dispose();
            }
        }
        public static void SpawnProjectile<T>(Vector2 position) where T : Projectile, new()
        {
            T Projectile = new T
            {
                Layer = LayerHandler.CurrentLayer
            };
            Projectile.SetDefaults();
            Projectile.Position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Center, frame, Color.White, 0f, frame.Size.ToVector2() / 2, 1f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            Utils.DrawRectangle(Position, Width, Height, Color.Green);
        }
        protected virtual void SetDefaults()
        {

        }
    }
}
