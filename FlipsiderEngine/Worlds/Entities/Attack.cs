using Flipsider.Assets;
using Flipsider.Worlds.Collision;
using Microsoft.Xna.Framework.Graphics;

namespace Flipsider.Worlds.Entities
{
    public abstract class Attack : PhysicalEntity
    {
        private readonly object? sender;
        private readonly string? message;

        protected Attack(double damage, object? sender, string? message) : base(true)
        {
            this.sender = sender;
            this.message = message;
            Damage = damage;
        }

        public double Damage { get; }

        public override void Intersect(ICollideable other)
        {
            if (other is IHurtable hurtable)
            {
                hurtable.Damage(new DamageSource(sender ?? this, message ?? "Attacked by " + GetType().Name, Damage));
            }
        }

        public static Attack Quick(Asset<Texture2D> texture, double damage, object? sender, string? message)
        {
            return new QuickAttack(texture, damage, sender, message);
        }

        private class QuickAttack : Attack
        {
            public QuickAttack(Asset<Texture2D> texture, double damage, object? sender, string? message) : base(damage, sender, message)
            {
                OnDraw += sb => Draw(sb, texture);
            }
        }
    }
}
