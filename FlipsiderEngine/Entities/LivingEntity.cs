using Flipsider.Assets;
using Flipsider.Extensions;
using Flipsider.Collision;
using Flipsider.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Flipsider.Graphics;

namespace Flipsider.Entities
{
    public delegate void DamageDelegate(DamageSource source);

    public abstract class LivingEntity : PhysicalEntity, IHurtable, ITileCollideable
    {
        protected LivingEntity(Asset<Texture2D> texture, float lifeMaxBase) : base(true)
        {
            Life = LifeMax = lifeMaxBase;
            OnUpdate += RemoveIfLifeEmpty;
            OnDraw += Draw;
            OnRemove += delegate 
            {
                OnDamage = null;
                OnDie = null;
            };
            Texture = texture;
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
        public double LifeMax { get; protected set; }
        /// <summary>
        /// An additional modifier to <see cref="LifeMax"/>. May be positive, negative, or zero.
        /// </summary>
        public double LifeMaxBonus { get; set; }
        /// <summary>
        /// If true, the entity will ignore calls to <see cref="Damage(DamageSource)"/>. Its Life can still be set, however.
        /// </summary>
        public bool ImmuneToDamage { get; set; }
        /// <summary>
        /// This entity's texture.
        /// </summary>
        public Asset<Texture2D>? Texture { get; protected set; }

        /// <summary>
        /// Called when this entity dies.
        /// </summary>
        public event Action? OnDie;

        /// <summary>
        /// Called when this entity takes damage.
        /// </summary>
        public event DamageDelegate? OnDamage;

        /// <summary>
        /// All tiles that this entity is touching.
        /// </summary>
        public IEnumerable<Tile> TouchingTiles { get; private set; } = Enumerable.Empty<Tile>();

        IEnumerable<Tile> ITileCollideable.Touching { set => TouchingTiles = value; }

        /// <summary>
        /// Does damage to this NPC.
        /// </summary>
        /// <param name="source">The damage source.</param>
        public void Damage(DamageSource source)
        {
            if (!ImmuneToDamage)
            {
                OnDamage?.Invoke(source);
                Life -= source.Amount;
            }
        }

        public virtual void Jump(float strength)
        {

        }

        protected void RemoveIfLifeEmpty(WorldEntity me)
        {
            if (life == 0)
                Delete();
        }

        protected virtual void Draw(WorldEntity me, SafeSpriteBatch sb)
        {
            if (Texture != null)
                Draw(sb, Texture);
        }
    }
}
