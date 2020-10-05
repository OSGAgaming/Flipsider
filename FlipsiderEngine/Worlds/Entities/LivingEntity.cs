using Flipsider.Core;
using Flipsider.Worlds.Collision;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Flipsider.Worlds.Entities
{
    public delegate void DamageDelegate(DamageSource source);

    public abstract class LivingEntity : PhysicalEntity
    {
        protected LivingEntity(float lifeMaxBase, bool registerHitbox)
        {
            Life = LifeMax = lifeMaxBase;
            OnUpdate += DeactivateIfLifeEmpty;
            OnRemove += delegate 
            {
                OnDamage = null;
                OnDie = null;
            };

            if (registerHitbox)
            {
                HitBox hb = new HitBox(this, OnHit, () => new RectangleF { Size = Size, Center = Center });
                OnSpawn += delegate { CurrentWorld.Collision.Add(hb); };
                OnRemove += delegate { CurrentWorld.Collision.Remove(hb); };
            }
        }

        private double life;
        /// <summary>
        /// How close this entity is to dying.
        /// </summary>
        public double Life
        {
            get => life;
            set
            {
                life = Math.Clamp(value, 0, LifeMax + LifeMaxBonus);
                if (OnDie != null && life == 0)
                    OnDie.Invoke();
            }
        }
        /// <summary>
        /// The maximum life for this entity.
        /// </summary>
        public double LifeMax { get; }
        /// <summary>
        /// An additional modifier to <see cref="LifeMax"/>. May be positive, negative, or zero.
        /// </summary>
        public double LifeMaxBonus { get; set; }
        /// <summary>
        /// If true, the entity will ignore calls to <see cref="Damage(DamageSource)"/>. Its Life can still be set, however.
        /// </summary>
        public bool ImmuneToDamage { get; set; }

        public event Action? OnDie;

        public event DamageDelegate? OnDamage;
        public void Damage(DamageSource source)
        {
            if (!ImmuneToDamage)
            {
                OnDamage?.Invoke(source);
                Life -= source.Amount;
            }
        }

        protected void DeactivateIfLifeEmpty()
        {
            if (life == 0)
                Delete();
        }

        /// <summary>
        /// Called when this entity's hitbox (registered through the constructor) is collided with.
        /// </summary>
        protected virtual void OnHit(ICollideable source)
        {
            // TODO impl hit/hurt/collide boxes
        }
    }
}
