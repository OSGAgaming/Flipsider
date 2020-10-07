using Flipsider.Assets;
using Flipsider.Collision;
using Flipsider.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Entities
{
    public abstract class Attack : PhysicalEntity, ICollisionObserver
    {
        private readonly DamageSource dmgSource;

        protected Attack(DamageSource dmgSource) : base(false)
        {
            OnSpawn += delegate { InWorld.Collision.AddObserver(this); };
            OnRemove += delegate { InWorld.Collision.RemoveObserver(this); };
            this.dmgSource = dmgSource;
        }

        public virtual void Intersect(ICollideable other)
        {
            if (other is IHurtable hurtable)
            {
                hurtable.Damage(new DamageSource(dmgSource));
            }
        }

        public static Attack Quick(Asset<Texture2D> texture, DamageSource dmgSource)
        {
            return new QuickAttack(texture, dmgSource);
        }

        private class QuickAttack : Attack
        {
            public QuickAttack(Asset<Texture2D> texture, DamageSource dmgSource) : base(dmgSource)
            {
                OnDraw += sb => Draw(sb, texture);
            }
        }
    }
}
